using ires.Infrastructure.Data;
using ires.Infrastructure.Entities;
using System.Data.Entity;

namespace ires.Infrastructure.Seeders
{
    public class BankSeeder
    {
        private readonly DataContext _dataContext;

        public BankSeeder(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task Seed(int companyID, bool isewallet)
        {
            if (await _dataContext.banks.Where(x => x.companyid == companyID && x.isewallet == isewallet).AnyAsync())
                return;
            List<Bank> banks = new List<Bank> {
                new Bank{ name="BDO UNIBANK INC", companyid = companyID, isewallet= false },
                new Bank{ name="LAND BANK OF THE PHILIPPINES", companyid = companyID, isewallet= false },
                new Bank{ name="BANK OF THE PHIL ISLANDS", companyid = companyID, isewallet= false },
                new Bank{ name="METROPOLITAN BANK & TCO", companyid = companyID, isewallet= false },
                new Bank{ name="CHINA BANKING CORP", companyid = companyID, isewallet= false },
                new Bank{ name="PHIL NATIONAL BANK", companyid = companyID, isewallet= false },
                new Bank{ name="RIZAL COMM'L BANKING CORP", companyid = companyID, isewallet= false },
                new Bank{ name="DEVELOPMENT BANK OF THE PHIL", companyid = companyID, isewallet= false },
                new Bank{ name="UNION BANK OF THE PHILS", companyid = companyID, isewallet= false },
                new Bank{ name="SECURITY BANK CORP", companyid = companyID, isewallet= false },
                new Bank{ name="EAST WEST BANKING CORP", companyid = companyID, isewallet= false },
                new Bank{ name="CITIBANK, N.A.", companyid = companyID, isewallet= false },
                new Bank{ name="ASIA UNITED BANK CORPORATION", companyid = companyID, isewallet= false },
                new Bank{ name="HONGKONG & SHANGHAI BANKING CORP", companyid = companyID, isewallet= false },
                new Bank{ name="BANK OF COMMERCE", companyid = companyID, isewallet= false },
                new Bank{ name="ROBINSONS BANK CORPORATION", companyid = companyID, isewallet= false },
                new Bank{ name="PHIL TRUST COMPANY", companyid = companyID, isewallet= false },
                new Bank{ name="PHIL BANK OF COMMUNICATIONS", companyid = companyID, isewallet= false },
                new Bank{ name="STANDARD CHARTERED BANK", companyid = companyID, isewallet= false },
                new Bank{ name="MAYBANK PHILIPPINES INCORPORATED", companyid = companyID, isewallet= false },
                new Bank{ name="MUFG BANK, LTD.", companyid = companyID, isewallet= false },
                new Bank{ name="MIZUHO BANK LTD - MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="BANK OF CHINA (HONGKONG) LIMITED-MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="CTBC BANK (PHILIPPINES) CORP", companyid = companyID, isewallet= false },
                new Bank{ name="PHILIPPINE VETERANS BANK", companyid = companyID, isewallet= false },
                new Bank{ name="JP MORGAN CHASE BANK NATIONAL ASSN.", companyid = companyID, isewallet= false },
                new Bank{ name="DEUTSCHE BANK AG", companyid = companyID, isewallet= false },
                new Bank{ name="AUSTRALIA AND NEW ZEALAND BANKING GROUP LIMITED", companyid = companyID, isewallet= false },
                new Bank{ name="SUMITOMO MITSUI BANKING CORPORATION-MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="BDO PRIVATE BANK, INC.", companyid = companyID, isewallet= false },
                new Bank{ name="BANK OF AMERICA N.A.", companyid = companyID, isewallet= false },
                new Bank{ name="ING BANK N.V.", companyid = companyID, isewallet= false },
                new Bank{ name="CIMB BANK PHILIPPINES INC", companyid = companyID, isewallet= false },
                new Bank{ name="KEB HANA BANK - MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="MEGA INT'L COMM'L BANK CO LTD", companyid = companyID, isewallet= false },
                new Bank{ name="INDUSTRIAL AND COMMERCIAL BANK OF CHINA LIMITED - MANILA BRA", companyid = companyID, isewallet= false },
                new Bank{ name="BANGKOK BANK PUBLIC CO LTD", companyid = companyID, isewallet= false },
                new Bank{ name="SHINHAN BANK - MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="INDUSTRIAL BANK OF KOREA MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="HUA NAN COMMERCIAL BANK LTD MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="CATHAY UNITED BANK CO LTD - MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="CHANG HWA COMMERCIAL BANK LTD - MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="UNITED OVERSEAS BANK LIMITED MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="FIRST COMMERCIAL BANK LTD MANILA BRANCH", companyid = companyID, isewallet= false },
                new Bank{ name="AL-AMANAH ISLAMIC INVESTMENT BANK OF THE PHILS", companyid = companyID, isewallet= false },
                new Bank{ name="GCASH", companyid = companyID, isewallet= true },
                new Bank{ name="PAYMAYA", companyid = companyID, isewallet= true },
            };
            _dataContext.AddRange(banks.Where(x => x.isewallet == isewallet));
            await _dataContext.SaveChangesAsync();
        }
    }
}