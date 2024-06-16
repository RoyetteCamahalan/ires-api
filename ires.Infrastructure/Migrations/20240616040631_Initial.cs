using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    accountid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    accountname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    memo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pettycashbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.accountid);
                });

            migrationBuilder.CreateTable(
                name: "applicationmodules",
                columns: table => new
                {
                    moduleid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modulename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    moduletypeid = table.Column<int>(type: "int", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicationmodules", x => x.moduleid);
                });

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    documentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    invoiceno = table.Column<long>(type: "bigint", nullable: false),
                    lotid = table.Column<long>(type: "bigint", nullable: false),
                    documentname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    filetype = table.Column<int>(type: "int", nullable: false),
                    isdeleted = table.Column<bool>(type: "bit", nullable: false),
                    attachedby = table.Column<long>(type: "bigint", nullable: false),
                    dateattached = table.Column<DateTime>(type: "datetime2", nullable: true),
                    documenttypeid = table.Column<int>(type: "int", nullable: false),
                    typeid = table.Column<int>(type: "int", nullable: false),
                    filesize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    filename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachments", x => x.documentid);
                });

            migrationBuilder.CreateTable(
                name: "bank",
                columns: table => new
                {
                    bankid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isewallet = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank", x => x.bankid);
                });

            migrationBuilder.CreateTable(
                name: "bill",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    billdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    particular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datefrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dateend = table.Column<DateTime>(type: "datetime2", nullable: true),
                    duedate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    paymentmode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentrefno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    checkouturl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentid = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cartypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    custid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    lname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contactno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tinnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.custid);
                });

            migrationBuilder.CreateTable(
                name: "expensetypecategory",
                columns: table => new
                {
                    expensecatid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expensetypecategory", x => x.expensecatid);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    moduleid = table.Column<int>(type: "int", nullable: false),
                    logtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    logAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    logdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    employeeid = table.Column<long>(type: "bigint", nullable: false),
                    withadmin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lot",
                columns: table => new
                {
                    lot_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    propertyid = table.Column<long>(type: "bigint", nullable: false),
                    blockno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lotno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pricepersquare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    default_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    min_down = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    compercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    commissionableamount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    housearea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    parkingarea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    comatdown = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    blocknoint = table.Column<int>(type: "int", nullable: false),
                    lotnoint = table.Column<int>(type: "int", nullable: false),
                    titleno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lot", x => x.lot_id);
                });

            migrationBuilder.CreateTable(
                name: "maintenancetypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenancetypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeeid = table.Column<long>(type: "bigint", nullable: false),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isread = table.Column<bool>(type: "bit", nullable: false),
                    typeid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "otherfees",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createdby = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: true),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otherfees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "property",
                columns: table => new
                {
                    propertyid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    propertyname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    alias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isjointventure = table.Column<bool>(type: "bit", nullable: false),
                    computationtype = table.Column<int>(type: "int", nullable: false),
                    defaultcommission = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    com_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    compercentageoverterm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    paymentterm = table.Column<int>(type: "int", nullable: false),
                    interest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    commissionterm = table.Column<int>(type: "int", nullable: false),
                    paymentextension = table.Column<int>(type: "int", nullable: false),
                    allow_straight_monthly = table.Column<int>(type: "int", nullable: false),
                    withholding = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    interesttype = table.Column<int>(type: "int", nullable: false),
                    addoninterestpermonth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    projectypeid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_property", x => x.propertyid);
                });

            migrationBuilder.CreateTable(
                name: "rentalAccountHistories",
                columns: table => new
                {
                    paymentdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    refno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    particular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    interest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    runningbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    chargedate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    seq = table.Column<int>(type: "int", nullable: false),
                    chargeid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "subscriptionplan",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    moduleid = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    storage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    surveylimit = table.Column<int>(type: "int", nullable: false),
                    monthlysubscription = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscriptionplan", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vendors",
                columns: table => new
                {
                    vendorid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    vendorname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contactno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tinno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendors", x => x.vendorid);
                });

            migrationBuilder.CreateTable(
                name: "pettycashdisbursement",
                columns: table => new
                {
                    disbursementid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    accountid = table.Column<long>(type: "bigint", nullable: false),
                    refdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    refno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transtype = table.Column<int>(type: "int", nullable: false),
                    refaccountid = table.Column<long>(type: "bigint", nullable: false),
                    refdisbursementid = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pettycashdisbursement", x => x.disbursementid);
                    table.ForeignKey(
                        name: "FK_pettycashdisbursement_accounts_accountid",
                        column: x => x.accountid,
                        principalTable: "accounts",
                        principalColumn: "accountid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pettycashdisbursement_accounts_refaccountid",
                        column: x => x.refaccountid,
                        principalTable: "accounts",
                        principalColumn: "accountid");
                });

            migrationBuilder.CreateTable(
                name: "planmodules",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    moduleid = table.Column<int>(type: "int", nullable: false),
                    planid = table.Column<int>(type: "int", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_planmodules", x => x.id);
                    table.ForeignKey(
                        name: "FK_planmodules_applicationmodules_moduleid",
                        column: x => x.moduleid,
                        principalTable: "applicationmodules",
                        principalColumn: "moduleid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userprivileges",
                columns: table => new
                {
                    userprivid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    moduleid = table.Column<int>(type: "int", nullable: false),
                    userid = table.Column<long>(type: "bigint", nullable: false),
                    canadd = table.Column<bool>(type: "bit", nullable: false),
                    canedit = table.Column<bool>(type: "bit", nullable: false),
                    canview = table.Column<bool>(type: "bit", nullable: false),
                    canverify = table.Column<bool>(type: "bit", nullable: false),
                    canaccess = table.Column<bool>(type: "bit", nullable: false),
                    canvoid = table.Column<bool>(type: "bit", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userprivileges", x => x.userprivid);
                    table.ForeignKey(
                        name: "FK_userprivileges_applicationmodules_moduleid",
                        column: x => x.moduleid,
                        principalTable: "applicationmodules",
                        principalColumn: "moduleid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bankaccounts",
                columns: table => new
                {
                    accountid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    accountname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankid = table.Column<long>(type: "bigint", nullable: false),
                    bankpreferredbranch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bankaccounts", x => x.accountid);
                    table.ForeignKey(
                        name: "FK_bankaccounts_bank_bankid",
                        column: x => x.bankid,
                        principalTable: "bank",
                        principalColumn: "bankid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    typeid = table.Column<int>(type: "int", nullable: false),
                    platenumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cars", x => x.id);
                    table.ForeignKey(
                        name: "FK_cars_cartypes_typeid",
                        column: x => x.typeid,
                        principalTable: "cartypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rentalcontracts",
                columns: table => new
                {
                    contractid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    contractno = table.Column<long>(type: "bigint", nullable: false),
                    custid = table.Column<long>(type: "bigint", nullable: false),
                    contractdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    montlyrent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    deposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    term = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    totalbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    noofmonthdeposit = table.Column<int>(type: "int", nullable: false),
                    noofmonthadvance = table.Column<int>(type: "int", nullable: false),
                    advancerent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ewtpercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    monthlypenalty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    penaltyextension = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    billingstart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentalcontracts", x => x.contractid);
                    table.ForeignKey(
                        name: "FK_rentalcontracts_customer_custid",
                        column: x => x.custid,
                        principalTable: "customer",
                        principalColumn: "custid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "survey",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    custid = table.Column<long>(type: "bigint", nullable: false),
                    owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    titleno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    surveyno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    surveydate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    propertyname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    landarea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contractprice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey", x => x.id);
                    table.ForeignKey(
                        name: "FK_survey_customer_custid",
                        column: x => x.custid,
                        principalTable: "customer",
                        principalColumn: "custid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expensetypes",
                columns: table => new
                {
                    expensetypeid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    expensetypedesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expensetypecat = table.Column<int>(type: "int", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expensetypes", x => x.expensetypeid);
                    table.ForeignKey(
                        name: "FK_expensetypes_expensetypecategory_expensetypecat",
                        column: x => x.expensetypecat,
                        principalTable: "expensetypecategory",
                        principalColumn: "expensecatid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "propertyrentals",
                columns: table => new
                {
                    propertyid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    projectid = table.Column<long>(type: "bigint", nullable: false),
                    propertyname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    monthlyrent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    alias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_propertyrentals", x => x.propertyid);
                    table.ForeignKey(
                        name: "FK_propertyrentals_property_projectid",
                        column: x => x.projectid,
                        principalTable: "property",
                        principalColumn: "propertyid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contactno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isactive = table.Column<bool>(type: "bit", nullable: true),
                    isverified = table.Column<bool>(type: "bit", nullable: false),
                    planid = table.Column<int>(type: "int", nullable: false),
                    subscriptionexpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    storage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    surveylimit = table.Column<int>(type: "int", nullable: false),
                    billingcycle = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.id);
                    table.ForeignKey(
                        name: "FK_company_subscriptionplan_planid",
                        column: x => x.planid,
                        principalTable: "subscriptionplan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    clientid = table.Column<long>(type: "bigint", nullable: false),
                    carid = table.Column<long>(type: "bigint", nullable: false),
                    startdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    enddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    noofdays = table.Column<int>(type: "int", nullable: false),
                    ratetype = table.Column<int>(type: "int", nullable: false),
                    rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totalrate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    drivername = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    isselfdrive = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_bookings_cars_carid",
                        column: x => x.carid,
                        principalTable: "cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookings_customer_clientid",
                        column: x => x.clientid,
                        principalTable: "customer",
                        principalColumn: "custid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "carmaintenance",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    carid = table.Column<long>(type: "bigint", nullable: false),
                    startdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    enddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    typeid = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carmaintenance", x => x.id);
                    table.ForeignKey(
                        name: "FK_carmaintenance_cars_carid",
                        column: x => x.carid,
                        principalTable: "cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_carmaintenance_maintenancetypes_typeid",
                        column: x => x.typeid,
                        principalTable: "maintenancetypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "othercharges",
                columns: table => new
                {
                    chargeid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    surveyid = table.Column<long>(type: "bigint", nullable: false),
                    invoiceno = table.Column<long>(type: "bigint", nullable: false),
                    lotid = table.Column<long>(type: "bigint", nullable: false),
                    chargedate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    chargeamount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    interestamount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    chargetype = table.Column<long>(type: "bigint", nullable: false),
                    runningbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    interestype = table.Column<int>(type: "int", nullable: false),
                    interestpercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_othercharges", x => x.chargeid);
                    table.ForeignKey(
                        name: "FK_othercharges_otherfees_chargetype",
                        column: x => x.chargetype,
                        principalTable: "otherfees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_othercharges_survey_surveyid",
                        column: x => x.surveyid,
                        principalTable: "survey",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expenseposting",
                columns: table => new
                {
                    chargeid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    vendorid = table.Column<long>(type: "bigint", nullable: false),
                    accountid = table.Column<long>(type: "bigint", nullable: false),
                    dateposted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    actualdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    expensetypeid = table.Column<long>(type: "bigint", nullable: false),
                    refno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    memo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    runningbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenseposting", x => x.chargeid);
                    table.ForeignKey(
                        name: "FK_expenseposting_expensetypes_expensetypeid",
                        column: x => x.expensetypeid,
                        principalTable: "expensetypes",
                        principalColumn: "expensetypeid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_expenseposting_vendors_vendorid",
                        column: x => x.vendorid,
                        principalTable: "vendors",
                        principalColumn: "vendorid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    expenseid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    transno = table.Column<long>(type: "bigint", nullable: false),
                    accountid = table.Column<long>(type: "bigint", nullable: false),
                    expensetype = table.Column<long>(type: "bigint", nullable: false),
                    refno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    refdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    memo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    payeeid = table.Column<long>(type: "bigint", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    runningbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    usepettycash = table.Column<bool>(type: "bit", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenses", x => x.expenseid);
                    table.ForeignKey(
                        name: "FK_expenses_accounts_accountid",
                        column: x => x.accountid,
                        principalTable: "accounts",
                        principalColumn: "accountid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_expenses_expensetypes_expensetype",
                        column: x => x.expensetype,
                        principalTable: "expensetypes",
                        principalColumn: "expensetypeid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_expenses_vendors_payeeid",
                        column: x => x.payeeid,
                        principalTable: "vendors",
                        principalColumn: "vendorid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rentalcontractdetails",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contractid = table.Column<long>(type: "bigint", nullable: false),
                    propertyid = table.Column<long>(type: "bigint", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentalcontractdetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_rentalcontractdetails_propertyrentals_propertyid",
                        column: x => x.propertyid,
                        principalTable: "propertyrentals",
                        principalColumn: "propertyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rentalcontractdetails_rentalcontracts_contractid",
                        column: x => x.contractid,
                        principalTable: "rentalcontracts",
                        principalColumn: "contractid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    employeeid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    middlename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mobileno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    isappsysadmin = table.Column<bool>(type: "bit", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userpass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdbyid = table.Column<long>(type: "bigint", nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    passwordresettoken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.employeeid);
                    table.ForeignKey(
                        name: "FK_employees_company_companyid",
                        column: x => x.companyid,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    paymentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    paymentdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    custid = table.Column<long>(type: "bigint", nullable: false),
                    encodedby = table.Column<long>(type: "bigint", nullable: false),
                    orno = table.Column<long>(type: "bigint", nullable: false),
                    receipttype = table.Column<int>(type: "int", nullable: false),
                    receiptno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentmode = table.Column<int>(type: "int", nullable: false),
                    totalamount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tender = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    change = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    daterefunded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    replacementpaymentid = table.Column<long>(type: "bigint", nullable: false),
                    replacedreceipts = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transtype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paidby = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.paymentid);
                    table.ForeignKey(
                        name: "FK_payment_customer_custid",
                        column: x => x.custid,
                        principalTable: "customer",
                        principalColumn: "custid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payment_employees_encodedby",
                        column: x => x.encodedby,
                        principalTable: "employees",
                        principalColumn: "employeeid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "banktobank",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    paymentid = table.Column<long>(type: "bigint", nullable: false),
                    bankid = table.Column<long>(type: "bigint", nullable: false),
                    accountid = table.Column<long>(type: "bigint", nullable: false),
                    paymentdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    memo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    refno = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banktobank", x => x.id);
                    table.ForeignKey(
                        name: "FK_banktobank_bank_bankid",
                        column: x => x.bankid,
                        principalTable: "bank",
                        principalColumn: "bankid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_banktobank_payment_paymentid",
                        column: x => x.paymentid,
                        principalTable: "payment",
                        principalColumn: "paymentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "checks",
                columns: table => new
                {
                    checkid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    paymentid = table.Column<long>(type: "bigint", nullable: false),
                    checkno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankid = table.Column<long>(type: "bigint", nullable: false),
                    checkdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    accountnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    replacedcheckdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    replacedcheckno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datedeposited = table.Column<DateTime>(type: "datetime2", nullable: true),
                    depositaccount = table.Column<long>(type: "bigint", nullable: false),
                    memo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checks", x => x.checkid);
                    table.ForeignKey(
                        name: "FK_checks_bank_bankid",
                        column: x => x.bankid,
                        principalTable: "bank",
                        principalColumn: "bankid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_checks_payment_paymentid",
                        column: x => x.paymentid,
                        principalTable: "payment",
                        principalColumn: "paymentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paymentdetails",
                columns: table => new
                {
                    paymentdetailid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chargeid = table.Column<long>(type: "bigint", nullable: false),
                    otherfeeid = table.Column<long>(type: "bigint", nullable: false),
                    paymentid = table.Column<long>(type: "bigint", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    discount_type = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    defaultchargeid = table.Column<long>(type: "bigint", nullable: false),
                    runningbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    invoiceno = table.Column<long>(type: "bigint", nullable: false),
                    lotid = table.Column<long>(type: "bigint", nullable: false),
                    paymenttype = table.Column<long>(type: "bigint", nullable: false),
                    rentalchargeid = table.Column<long>(type: "bigint", nullable: false),
                    rentalid = table.Column<long>(type: "bigint", nullable: false),
                    remarks_deletion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payableType = table.Column<int>(type: "int", nullable: false),
                    surveyid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentdetails", x => x.paymentdetailid);
                    table.ForeignKey(
                        name: "FK_paymentdetails_payment_paymentid",
                        column: x => x.paymentid,
                        principalTable: "payment",
                        principalColumn: "paymentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "cartypes",
                columns: new[] { "id", "isactive", "name" },
                values: new object[,]
                {
                    { 1, true, "Hatchback" },
                    { 2, true, "Sedan" },
                    { 3, true, "Minivan" },
                    { 4, true, "Crossover" },
                    { 5, true, "Pickup" },
                    { 6, true, "SUV" },
                    { 7, true, "Van" },
                    { 8, true, "Others" }
                });

            migrationBuilder.InsertData(
                table: "maintenancetypes",
                columns: new[] { "id", "isactive", "name" },
                values: new object[,]
                {
                    { 1, true, "Repair and Maintenance" },
                    { 2, true, "Registration Renewal" },
                    { 3, true, "Personal Use" },
                    { 4, true, "Others" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_bankaccounts_bankid",
                table: "bankaccounts",
                column: "bankid");

            migrationBuilder.CreateIndex(
                name: "IX_banktobank_bankid",
                table: "banktobank",
                column: "bankid");

            migrationBuilder.CreateIndex(
                name: "IX_banktobank_paymentid",
                table: "banktobank",
                column: "paymentid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_carid",
                table: "bookings",
                column: "carid");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_clientid",
                table: "bookings",
                column: "clientid");

            migrationBuilder.CreateIndex(
                name: "IX_carmaintenance_carid",
                table: "carmaintenance",
                column: "carid");

            migrationBuilder.CreateIndex(
                name: "IX_carmaintenance_typeid",
                table: "carmaintenance",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "IX_cars_typeid",
                table: "cars",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "IX_checks_bankid",
                table: "checks",
                column: "bankid");

            migrationBuilder.CreateIndex(
                name: "IX_checks_paymentid",
                table: "checks",
                column: "paymentid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_company_planid",
                table: "company",
                column: "planid");

            migrationBuilder.CreateIndex(
                name: "IX_employees_companyid",
                table: "employees",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_expenseposting_expensetypeid",
                table: "expenseposting",
                column: "expensetypeid");

            migrationBuilder.CreateIndex(
                name: "IX_expenseposting_vendorid",
                table: "expenseposting",
                column: "vendorid");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_accountid",
                table: "expenses",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_expensetype",
                table: "expenses",
                column: "expensetype");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_payeeid",
                table: "expenses",
                column: "payeeid");

            migrationBuilder.CreateIndex(
                name: "IX_expensetypes_expensetypecat",
                table: "expensetypes",
                column: "expensetypecat");

            migrationBuilder.CreateIndex(
                name: "IX_othercharges_chargetype",
                table: "othercharges",
                column: "chargetype");

            migrationBuilder.CreateIndex(
                name: "IX_othercharges_surveyid",
                table: "othercharges",
                column: "surveyid");

            migrationBuilder.CreateIndex(
                name: "IX_payment_custid",
                table: "payment",
                column: "custid");

            migrationBuilder.CreateIndex(
                name: "IX_payment_encodedby",
                table: "payment",
                column: "encodedby");

            migrationBuilder.CreateIndex(
                name: "IX_paymentdetails_paymentid",
                table: "paymentdetails",
                column: "paymentid");

            migrationBuilder.CreateIndex(
                name: "IX_pettycashdisbursement_accountid",
                table: "pettycashdisbursement",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_pettycashdisbursement_refaccountid",
                table: "pettycashdisbursement",
                column: "refaccountid");

            migrationBuilder.CreateIndex(
                name: "IX_planmodules_moduleid",
                table: "planmodules",
                column: "moduleid");

            migrationBuilder.CreateIndex(
                name: "IX_propertyrentals_projectid",
                table: "propertyrentals",
                column: "projectid");

            migrationBuilder.CreateIndex(
                name: "IX_rentalcontractdetails_contractid",
                table: "rentalcontractdetails",
                column: "contractid");

            migrationBuilder.CreateIndex(
                name: "IX_rentalcontractdetails_propertyid",
                table: "rentalcontractdetails",
                column: "propertyid");

            migrationBuilder.CreateIndex(
                name: "IX_rentalcontracts_custid",
                table: "rentalcontracts",
                column: "custid");

            migrationBuilder.CreateIndex(
                name: "IX_survey_custid",
                table: "survey",
                column: "custid");

            migrationBuilder.CreateIndex(
                name: "IX_userprivileges_moduleid",
                table: "userprivileges",
                column: "moduleid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "bankaccounts");

            migrationBuilder.DropTable(
                name: "banktobank");

            migrationBuilder.DropTable(
                name: "bill");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "carmaintenance");

            migrationBuilder.DropTable(
                name: "checks");

            migrationBuilder.DropTable(
                name: "expenseposting");

            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "lot");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "othercharges");

            migrationBuilder.DropTable(
                name: "paymentdetails");

            migrationBuilder.DropTable(
                name: "pettycashdisbursement");

            migrationBuilder.DropTable(
                name: "planmodules");

            migrationBuilder.DropTable(
                name: "rentalAccountHistories");

            migrationBuilder.DropTable(
                name: "rentalcontractdetails");

            migrationBuilder.DropTable(
                name: "userprivileges");

            migrationBuilder.DropTable(
                name: "cars");

            migrationBuilder.DropTable(
                name: "maintenancetypes");

            migrationBuilder.DropTable(
                name: "bank");

            migrationBuilder.DropTable(
                name: "expensetypes");

            migrationBuilder.DropTable(
                name: "vendors");

            migrationBuilder.DropTable(
                name: "otherfees");

            migrationBuilder.DropTable(
                name: "survey");

            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "propertyrentals");

            migrationBuilder.DropTable(
                name: "rentalcontracts");

            migrationBuilder.DropTable(
                name: "applicationmodules");

            migrationBuilder.DropTable(
                name: "cartypes");

            migrationBuilder.DropTable(
                name: "expensetypecategory");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "property");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "company");

            migrationBuilder.DropTable(
                name: "subscriptionplan");
        }
    }
}
