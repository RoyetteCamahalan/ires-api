# Billing Account Notification CRON Job

## Overview

`BillingNotificationJob` is a scheduled background job that sends in-app due-date reminders for active billing accounts. It runs daily via Quartz.NET and creates `Notification` records for the appropriate recipients based on each account's `NotifyOption` setting.

---

## Schedule

| Setting | Value |
|---|---|
| CRON expression | `0 0 10 * * ?` |
| Runs at | 10:00 AM PST / 1:00 AM Manila (PHT) |
| Frequency | Once per day |

To change the schedule, edit `BillingNotificationJobSetup.cs`.

---

## Trigger Criteria

An account is eligible for notification when **all** of the following are true:

| Condition | Detail |
|---|---|
| `IsActive = true` | Account must be active |
| `NotifyOption != "None"` | Notifications must not be disabled |
| `NextDueDate` is set | Account must have a computed due date |
| `NextDueDate - NotifyDaysBefore <= today` | Notification window has been reached |
| `LastNotified` is `null` OR `< 2 days ago` | Prevents re-notifying every day within the same window |

### Cooldown Logic

The 2-day cooldown prevents the same account from flooding recipients with repeated notifications during the notification window. Once notified, `LastNotified` is stamped to `DateTime.UtcNow`. The account will not trigger again until that timestamp is older than 2 days.

---

## Notification Recipients

Controlled by the `NotifyOption` field on the billing account:

| `NotifyOption` | Who is notified |
|---|---|
| `"None"` | Nobody — account is skipped entirely |
| `"OnlyMe"` | The employee who created the billing account (`CreatedById`) |
| `"AllUsers"` | All **active** employees in the same company as the account |

See [`BillingNotification`](../ires.Domain/Enumerations/BillingNotification.cs) for constants.

---

## Notification Record

Each recipient gets a row inserted into the `notifications` table:

| Field | Value |
|---|---|
| `employeeid` | Recipient's employee ID |
| `details` | `"Billing account '{AccountName}' is due on {MMM dd, yyyy}."` |
| `url` | `/billing-accounts/{id}` |
| `typeid` | `25` (`AppModule.Billing`) |
| `isread` | `false` |
| `datecreated` | `DateTime.UtcNow` |

---

## Email Reminder

In addition to the in-app notification, an HTML email is sent to each recipient using the template `Templates/BillingAccountDueReminder.html`.

**Template placeholders**

| Placeholder | Replaced with |
|---|---|
| `{0}` | `uiBaseURL` from `appsettings.json` |
| `{user_firstname}` | Recipient's first name |
| `{account_name}` | Billing account name |
| `{due_date}` | Due date formatted as `MMM dd, yyyy` |

**Subject:** `Billing Due Reminder: {AccountName}`

Recipients with a blank or missing email address are silently skipped. Email delivery failures do not block in-app notification creation — they are caught and logged separately by `IMailService`.

---

## Involved Files

| File | Role |
|---|---|
| `ires.Infrastructure/Jobs/Billing/BillingNotificationJob.cs` | Quartz `IJob` implementation — calls `SendBillingAccountNotifications` |
| `ires.Infrastructure/Jobs/Billing/BillingNotificationJobSetup.cs` | Registers the job and CRON trigger with Quartz |
| `ires.Infrastructure/DependencyInjection.cs` | Registers `BillingNotificationJobSetup` via `ConfigureOptions` |
| `ires.Domain/Contracts/IAppService.cs` | Exposes `SendBillingAccountNotifications()` |
| `ires.Infrastructure/Repositories/AppRepository.cs` | Implements `SendBillingAccountNotifications()` — queries accounts, resolves recipients, inserts notifications, sends emails |
| `ires.Domain/Enumerations/BillingNotification.cs` | String constants: `None`, `OnlyMe`, `AllUsers` |
| `ires.Infrastructure/Entities/BillingAccount.cs` | Source entity; `NotifyOption`, `NotifyDaysBefore`, `NextDueDate`, `LastNotified` |
| `ires-api/Templates/BillingAccountDueReminder.html` | HTML email template with placeholders `{0}`, `{user_firstname}`, `{account_name}`, `{due_date}` |

---

## Flow Diagram

```
[Quartz Scheduler] (daily @ 10:00 AM PST)
		|
		v
BillingNotificationJob.Execute()
		|
		v
AppRepository.SendBillingAccountNotifications()
		|
		+-- Query: active accounts where (NextDueDate - NotifyDaysBefore) <= today
		|          AND NotifyOption != "None"
		|          AND (LastNotified is null OR LastNotified < 2 days ago)
		|          (IgnoreQueryFilters — processes ALL companies)
		|
		+-- For each account:
		|       |
		|       +-- NotifyOption = "OnlyMe"   -> recipient = CreatedById
		|       +-- NotifyOption = "AllUsers" -> recipients = all active employees in same company
		|
		+-- Insert Notification rows for each recipient
		+-- Send HTML email via BillingAccountDueReminder.html template
		+-- Stamp account.LastNotified = UtcNow
		|
		v
SaveChangesAsync()
```

---

## Notes

- The job uses `IgnoreQueryFilters()` when querying `billingAccounts` so the EF global company-scope filter is bypassed and accounts from **all companies** are processed in a single run.
- Employee queries for `AllUsers` do **not** use `IgnoreQueryFilters` because the `employees` table does not have a company-scoped global filter — it is filtered explicitly via `e.companyid == account.CompanyId`.
- Errors are caught per-job-run and logged via `ILogService`. A failure does not crash the scheduler.
