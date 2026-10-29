using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsUserDefined = table.Column<bool>(type: "INTEGER", nullable: false),
                    CategoryGroupName = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryGroupName", "CategoryName", "Description", "IsUserDefined" },
                values: new object[,]
                {
                    { new Guid("70666376-3200-0000-0000-000000000001"), "INCOME", "INCOME_CHILD_SUPPORT", "Child support refers to court-ordered payments made by a parent to financially support their child’s living expenses", false },
                    { new Guid("70666376-3200-0000-0000-000000000002"), "INCOME", "INCOME_CONTRACTOR", "Income from freelance or independent contract work.", false },
                    { new Guid("70666376-3200-0000-0000-000000000003"), "INCOME", "INCOME_DIVIDENDS", "Income from dividends", false },
                    { new Guid("70666376-3200-0000-0000-000000000004"), "INCOME", "INCOME_GIG_ECONOMY", "Money earned by working in the gig economy, for example by driving for Lyft, Uber, etc.", false },
                    { new Guid("70666376-3200-0000-0000-000000000005"), "INCOME", "INCOME_INTEREST_EARNED", "Income from interest on savings accounts", false },
                    { new Guid("70666376-3200-0000-0000-000000000006"), "INCOME", "INCOME_LONG_TERM_DISABILITY", "Disability payments, for example from social security.", false },
                    { new Guid("70666376-3200-0000-0000-000000000007"), "INCOME", "INCOME_MILITARY", "Money earned from veterans benefits. Salary earned from serving in the military (through DFAS) is categorized as salary", false },
                    { new Guid("70666376-3200-0000-0000-000000000008"), "INCOME", "INCOME_RENTAL", "Rental income includes money earned from payments related to property rentals, lease income, and short-term rental platforms such as airbnb and VRBO.", false },
                    { new Guid("70666376-3200-0000-0000-000000000009"), "INCOME", "INCOME_RETIREMENT_PENSION", "Payments from the social security administration, private retirement systems, (eg. 401k) pensions, and government retirement programs", false },
                    { new Guid("70666376-3200-0000-0000-000000000010"), "INCOME", "INCOME_SALARY", "Income from salaries and wages", false },
                    { new Guid("70666376-3200-0000-0000-000000000011"), "INCOME", "INCOME_TAX_REFUND", "Government tax refund provided to the user", false },
                    { new Guid("70666376-3200-0000-0000-000000000012"), "INCOME", "INCOME_UNEMPLOYMENT", "Money earned from unemployment benefits", false },
                    { new Guid("70666376-3200-0000-0000-000000000013"), "INCOME", "INCOME_OTHER", "Other miscellaneous income", false },
                    { new Guid("70666376-3200-0000-0000-000000000014"), "LOAN_DISBURSEMENTS", "LOAN_DISBURSEMENTS_AUTO", "Auto loan disbursements", false },
                    { new Guid("70666376-3200-0000-0000-000000000015"), "LOAN_DISBURSEMENTS", "LOAN_DISBURSEMENTS_CASH_ADVANCES", "Payday loans and cash advances", false },
                    { new Guid("70666376-3200-0000-0000-000000000016"), "LOAN_DISBURSEMENTS", "LOAN_DISBURSEMENTS_EWA", "Early wage access disbursements from integrated payroll providers.", false },
                    { new Guid("70666376-3200-0000-0000-000000000017"), "LOAN_DISBURSEMENTS", "LOAN_DISBURSEMENTS_MORTGAGE", "Mortgage loan disbursements", false },
                    { new Guid("70666376-3200-0000-0000-000000000018"), "LOAN_DISBURSEMENTS", "LOAN_DISBURSEMENTS_PERSONAL", "Personal loan disbursements, including both secured and unsecured loans.", false },
                    { new Guid("70666376-3200-0000-0000-000000000019"), "LOAN_DISBURSEMENTS", "LOAN_DISBURSEMENTS_STUDENT", "Student loan disbursements.", false },
                    { new Guid("70666376-3200-0000-0000-000000000020"), "LOAN_DISBURSEMENTS", "LOAN_DISBURSEMENTS_OTHER_DISBURSEMENT", "Other miscellaneous loan disbursements", false },
                    { new Guid("70666376-3200-0000-0000-000000000021"), "LOAN_PAYMENTS", "LOAN_PAYMENTS_BNPL", "Loan payments for buy now, pay later (BNPL) services.", false },
                    { new Guid("70666376-3200-0000-0000-000000000022"), "LOAN_PAYMENTS", "LOAN_PAYMENTS_CAR_PAYMENT", "Payments on car loans and leases.", false },
                    { new Guid("70666376-3200-0000-0000-000000000023"), "LOAN_PAYMENTS", "LOAN_PAYMENTS_CASH_ADVANCES", "Loan payments for cash advances.", false },
                    { new Guid("70666376-3200-0000-0000-000000000024"), "LOAN_PAYMENTS", "LOAN_PAYMENTS_CREDIT_CARD_PAYMENT", "Payments made to a credit card account.", false },
                    { new Guid("70666376-3200-0000-0000-000000000025"), "LOAN_PAYMENTS", "LOAN_PAYMENTS_EWA", "Loan payments for early wage access from integrated payroll providers.", false },
                    { new Guid("70666376-3200-0000-0000-000000000026"), "LOAN_PAYMENTS", "LOAN_PAYMENTS_MORTGAGE_PAYMENT", "Payments on mortgages", false },
                    { new Guid("70666376-3200-0000-0000-000000000027"), "LOAN_PAYMENTS", "LOAN_PAYMENTS_PERSONAL_LOAN_PAYMENT", "Payments on personal loans.", false },
                    { new Guid("70666376-3200-0000-0000-000000000028"), "LOAN_PAYMENTS", "LOAN_PAYMENTS_STUDENT_LOAN_PAYMENT", "Payments on student loans. For college tuition, refer to \"General Services - Education\"", false },
                    { new Guid("70666376-3200-0000-0000-000000000029"), "LOAN_PAYMENTS", "LOAN_PAYMENTS_OTHER_PAYMENT", "Other miscellaneous debt payments", false },
                    { new Guid("70666376-3200-0000-0000-000000000030"), "TRANSFER_IN", "TRANSFER_IN_ACCOUNT_TRANSFER", "General inbound transfers from another account", false },
                    { new Guid("70666376-3200-0000-0000-000000000031"), "TRANSFER_IN", "TRANSFER_IN_DEPOSIT", "Cash, checks, and ATM deposits into a bank account", false },
                    { new Guid("70666376-3200-0000-0000-000000000032"), "TRANSFER_IN", "TRANSFER_IN_INVESTMENT_AND_RETIREMENT_FUNDS", "Money transferred into the account from an investment or retirement account", false },
                    { new Guid("70666376-3200-0000-0000-000000000033"), "TRANSFER_IN", "TRANSFER_IN_SAVINGS", "Inbound transfers to a savings account", false },
                    { new Guid("70666376-3200-0000-0000-000000000034"), "TRANSFER_IN", "TRANSFER_IN_TRANSFER_IN_FROM_APPS", "Money transferred into the account from another application", false },
                    { new Guid("70666376-3200-0000-0000-000000000035"), "TRANSFER_IN", "TRANSFER_IN_WIRE", "Money transferred into the account from a wire transfer from another bank", false },
                    { new Guid("70666376-3200-0000-0000-000000000036"), "TRANSFER_IN", "TRANSFER_IN_OTHER_TRANSFER_IN", "Other miscellaneous inbound transactions", false },
                    { new Guid("70666376-3200-0000-0000-000000000037"), "TRANSFER_OUT", "TRANSFER_OUT_ACCOUNT_TRANSFER", "General outbound transfers to another account", false },
                    { new Guid("70666376-3200-0000-0000-000000000038"), "TRANSFER_OUT", "TRANSFER_OUT_CRYPTO", "Outbound transfers of cryptocurrency", false },
                    { new Guid("70666376-3200-0000-0000-000000000039"), "TRANSFER_OUT", "TRANSFER_OUT_INVESTMENT_AND_RETIREMENT_FUNDS", "Transfers to an investment or retirement account, including investment apps such as Acorns, Betterment", false },
                    { new Guid("70666376-3200-0000-0000-000000000040"), "TRANSFER_OUT", "TRANSFER_OUT_SAVINGS", "Outbound transfers to savings accounts", false },
                    { new Guid("70666376-3200-0000-0000-000000000041"), "TRANSFER_OUT", "TRANSFER_OUT_TRANSFER_OUT_FROM_APPS", "Money transferred out of the account into another application", false },
                    { new Guid("70666376-3200-0000-0000-000000000042"), "TRANSFER_OUT", "TRANSFER_OUT_WIRE", "Money transferred out of the account from a wire transfer to another bank", false },
                    { new Guid("70666376-3200-0000-0000-000000000043"), "TRANSFER_OUT", "TRANSFER_OUT_WITHDRAWAL", "Withdrawals from a bank account", false },
                    { new Guid("70666376-3200-0000-0000-000000000044"), "TRANSFER_OUT", "TRANSFER_OUT_OTHER_TRANSFER_OUT", "Other miscellaneous outbound transactions", false },
                    { new Guid("70666376-3200-0000-0000-000000000045"), "BANK_FEES", "BANK_FEES_ATM_FEES", "Fees incurred for out-of-network ATMs", false },
                    { new Guid("70666376-3200-0000-0000-000000000046"), "BANK_FEES", "BANK_FEES_INSUFFICIENT_FUNDS", "Fees relating to insufficient funds", false },
                    { new Guid("70666376-3200-0000-0000-000000000047"), "BANK_FEES", "BANK_FEES_INTEREST_CHARGE", "Fees incurred for interest on purchases (this excludes cash advance interest fee)", false },
                    { new Guid("70666376-3200-0000-0000-000000000048"), "BANK_FEES", "BANK_FEES_FOREIGN_TRANSACTION_FEES", "Fees incurred on non-domestic transactions", false },
                    { new Guid("70666376-3200-0000-0000-000000000049"), "BANK_FEES", "BANK_FEES_OVERDRAFT_FEES", "Penalty payment for overdrafts", false },
                    { new Guid("70666376-3200-0000-0000-000000000050"), "BANK_FEES", "BANK_FEES_LATE_FEES", "Penalty payment for late payment", false },
                    { new Guid("70666376-3200-0000-0000-000000000051"), "BANK_FEES", "BANK_FEES_CASH_ADVANCE", "Fees incurred for withdrawing cash using a credit card, including transaction fees and interest fees.", false },
                    { new Guid("70666376-3200-0000-0000-000000000052"), "BANK_FEES", "BANK_FEES_OTHER_BANK_FEES", "Other miscellaneous bank fees, including annual fee", false },
                    { new Guid("70666376-3200-0000-0000-000000000053"), "ENTERTAINMENT", "ENTERTAINMENT_CASINOS_AND_GAMBLING", "Gambling, casinos, and sports betting", false },
                    { new Guid("70666376-3200-0000-0000-000000000054"), "ENTERTAINMENT", "ENTERTAINMENT_MUSIC_AND_AUDIO", "Digital and in-person music purchases, including music streaming services", false },
                    { new Guid("70666376-3200-0000-0000-000000000055"), "ENTERTAINMENT", "ENTERTAINMENT_SPORTING_EVENTS_AMUSEMENT_PARKS_AND_MUSEUMS", "Purchases made at sporting events, music venues, concerts, museums, and amusement parks", false },
                    { new Guid("70666376-3200-0000-0000-000000000056"), "ENTERTAINMENT", "ENTERTAINMENT_TV_AND_MOVIES", "In home movie streaming services and movie theaters", false },
                    { new Guid("70666376-3200-0000-0000-000000000057"), "ENTERTAINMENT", "ENTERTAINMENT_VIDEO_GAMES", "Digital and in-person video game purchases", false },
                    { new Guid("70666376-3200-0000-0000-000000000058"), "ENTERTAINMENT", "ENTERTAINMENT_OTHER_ENTERTAINMENT", "Other miscellaneous entertainment purchases, including night life and adult entertainment", false },
                    { new Guid("70666376-3200-0000-0000-000000000059"), "FOOD_AND_DRINK", "FOOD_AND_DRINK_BEER_WINE_AND_LIQUOR", "Beer, wine, and liquor stores.", false },
                    { new Guid("70666376-3200-0000-0000-000000000060"), "FOOD_AND_DRINK", "FOOD_AND_DRINK_COFFEE", "Purchases at coffee shops or cafes", false },
                    { new Guid("70666376-3200-0000-0000-000000000061"), "FOOD_AND_DRINK", "FOOD_AND_DRINK_FAST_FOOD", "Dining expenses for fast food chains", false },
                    { new Guid("70666376-3200-0000-0000-000000000062"), "FOOD_AND_DRINK", "FOOD_AND_DRINK_GROCERIES", "Purchases for fresh produce and groceries, including farmers' markets", false },
                    { new Guid("70666376-3200-0000-0000-000000000063"), "FOOD_AND_DRINK", "FOOD_AND_DRINK_RESTAURANT", "Dining expenses for restaurants, bars, gastropubs, and diners", false },
                    { new Guid("70666376-3200-0000-0000-000000000064"), "FOOD_AND_DRINK", "FOOD_AND_DRINK_VENDING_MACHINES", "Purchases made at vending machine operators", false },
                    { new Guid("70666376-3200-0000-0000-000000000065"), "FOOD_AND_DRINK", "FOOD_AND_DRINK_OTHER_FOOD_AND_DRINK", "Other miscellaneous food and drink, including desserts, juice bars, and delis", false },
                    { new Guid("70666376-3200-0000-0000-000000000066"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_BOOKSTORES_AND_NEWSSTANDS", "Books, magazines, and news", false },
                    { new Guid("70666376-3200-0000-0000-000000000067"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_CLOTHING_AND_ACCESSORIES", "Apparel, shoes, and jewelry", false },
                    { new Guid("70666376-3200-0000-0000-000000000068"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_CONVENIENCE_STORES", "Purchases at convenience stores", false },
                    { new Guid("70666376-3200-0000-0000-000000000069"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_DEPARTMENT_STORES", "Retail stores with wide ranges of consumer goods, typically specializing in clothing and home goods", false },
                    { new Guid("70666376-3200-0000-0000-000000000070"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_DISCOUNT_STORES", "Stores selling goods at a discounted price", false },
                    { new Guid("70666376-3200-0000-0000-000000000071"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_ELECTRONICS", "Electronics stores and websites", false },
                    { new Guid("70666376-3200-0000-0000-000000000072"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_GIFTS_AND_NOVELTIES", "Photo, gifts, cards, and floral stores", false },
                    { new Guid("70666376-3200-0000-0000-000000000073"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_OFFICE_SUPPLIES", "Stores that specialize in office goods", false },
                    { new Guid("70666376-3200-0000-0000-000000000074"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_ONLINE_MARKETPLACES", "Multi-purpose e-commerce platforms such as Etsy, Ebay and Amazon", false },
                    { new Guid("70666376-3200-0000-0000-000000000075"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_PET_SUPPLIES", "Pet supplies and pet food", false },
                    { new Guid("70666376-3200-0000-0000-000000000076"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_SPORTING_GOODS", "Sporting goods, camping gear, and outdoor equipment", false },
                    { new Guid("70666376-3200-0000-0000-000000000077"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_SUPERSTORES", "Superstores such as Target and Walmart, selling both groceries and general merchandise", false },
                    { new Guid("70666376-3200-0000-0000-000000000078"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_TOBACCO_AND_VAPE", "Purchases for tobacco and vaping products", false },
                    { new Guid("70666376-3200-0000-0000-000000000079"), "GENERAL_MERCHANDISE", "GENERAL_MERCHANDISE_OTHER_GENERAL_MERCHANDISE", "Other miscellaneous merchandise, including toys, hobbies, and arts and crafts", false },
                    { new Guid("70666376-3200-0000-0000-000000000080"), "HOME_IMPROVEMENT", "HOME_IMPROVEMENT_FURNITURE", "Furniture, bedding, and home accessories", false },
                    { new Guid("70666376-3200-0000-0000-000000000081"), "HOME_IMPROVEMENT", "HOME_IMPROVEMENT_HARDWARE", "Building materials, hardware stores, paint, and wallpaper", false },
                    { new Guid("70666376-3200-0000-0000-000000000082"), "HOME_IMPROVEMENT", "HOME_IMPROVEMENT_REPAIR_AND_MAINTENANCE", "Plumbing, lighting, gardening, and roofing", false },
                    { new Guid("70666376-3200-0000-0000-000000000083"), "HOME_IMPROVEMENT", "HOME_IMPROVEMENT_SECURITY", "Home security system purchases", false },
                    { new Guid("70666376-3200-0000-0000-000000000084"), "HOME_IMPROVEMENT", "HOME_IMPROVEMENT_OTHER_HOME_IMPROVEMENT", "Other miscellaneous home purchases, including pool installation and pest control", false },
                    { new Guid("70666376-3200-0000-0000-000000000085"), "MEDICAL", "MEDICAL_DENTAL_CARE", "Dentists and general dental care", false },
                    { new Guid("70666376-3200-0000-0000-000000000086"), "MEDICAL", "MEDICAL_EYE_CARE", "Optometrists, contacts, and glasses stores", false },
                    { new Guid("70666376-3200-0000-0000-000000000087"), "MEDICAL", "MEDICAL_NURSING_CARE", "Nursing care and facilities", false },
                    { new Guid("70666376-3200-0000-0000-000000000088"), "MEDICAL", "MEDICAL_PHARMACIES_AND_SUPPLEMENTS", "Pharmacies and nutrition shops", false },
                    { new Guid("70666376-3200-0000-0000-000000000089"), "MEDICAL", "MEDICAL_PRIMARY_CARE", "Doctors and physicians", false },
                    { new Guid("70666376-3200-0000-0000-000000000090"), "MEDICAL", "MEDICAL_VETERINARY_SERVICES", "Prevention and care procedures for animals", false },
                    { new Guid("70666376-3200-0000-0000-000000000091"), "MEDICAL", "MEDICAL_OTHER_MEDICAL", "Other miscellaneous medical, including blood work, hospitals, and ambulances", false },
                    { new Guid("70666376-3200-0000-0000-000000000092"), "PERSONAL_CARE", "PERSONAL_CARE_GYMS_AND_FITNESS_CENTERS", "Gyms, fitness centers, and workout classes", false },
                    { new Guid("70666376-3200-0000-0000-000000000093"), "PERSONAL_CARE", "PERSONAL_CARE_HAIR_AND_BEAUTY", "Manicures, haircuts, waxing, spa/massages, and bath and beauty products", false },
                    { new Guid("70666376-3200-0000-0000-000000000094"), "PERSONAL_CARE", "PERSONAL_CARE_LAUNDRY_AND_DRY_CLEANING", "Wash and fold, and dry cleaning expenses", false },
                    { new Guid("70666376-3200-0000-0000-000000000095"), "PERSONAL_CARE", "PERSONAL_CARE_OTHER_PERSONAL_CARE", "Other miscellaneous personal care, including mental health apps and services", false },
                    { new Guid("70666376-3200-0000-0000-000000000096"), "GENERAL_SERVICES", "GENERAL_SERVICES_ACCOUNTING_AND_FINANCIAL_PLANNING", "Financial planning, tax, and accounting services.", false },
                    { new Guid("70666376-3200-0000-0000-000000000097"), "GENERAL_SERVICES", "GENERAL_SERVICES_AUTOMOTIVE", "Oil changes, car washes, repairs, and towing", false },
                    { new Guid("70666376-3200-0000-0000-000000000098"), "GENERAL_SERVICES", "GENERAL_SERVICES_CHILDCARE", "Babysitters and daycare", false },
                    { new Guid("70666376-3200-0000-0000-000000000099"), "GENERAL_SERVICES", "GENERAL_SERVICES_CONSULTING_AND_LEGAL", "Consulting and legal services", false },
                    { new Guid("70666376-3200-0000-0000-000000000100"), "GENERAL_SERVICES", "GENERAL_SERVICES_EDUCATION", "Elementary, high school, professional schools, and college tuition", false },
                    { new Guid("70666376-3200-0000-0000-000000000101"), "GENERAL_SERVICES", "GENERAL_SERVICES_INSURANCE", "Insurance for auto, home, and healthcare", false },
                    { new Guid("70666376-3200-0000-0000-000000000102"), "GENERAL_SERVICES", "GENERAL_SERVICES_POSTAGE_AND_SHIPPING", "Mail, packaging, and shipping services", false },
                    { new Guid("70666376-3200-0000-0000-000000000103"), "GENERAL_SERVICES", "GENERAL_SERVICES_STORAGE", "Storage services and facilities", false },
                    { new Guid("70666376-3200-0000-0000-000000000104"), "GENERAL_SERVICES", "GENERAL_SERVICES_OTHER_GENERAL_SERVICES", "Other miscellaneous services, including advertising and cloud storage", false },
                    { new Guid("70666376-3200-0000-0000-000000000105"), "GOVERNMENT_AND_NON_PROFIT", "GOVERNMENT_AND_NON_PROFIT_DONATIONS", "Charitable, political, and religious donations", false },
                    { new Guid("70666376-3200-0000-0000-000000000106"), "GOVERNMENT_AND_NON_PROFIT", "GOVERNMENT_AND_NON_PROFIT_GOVERNMENT_DEPARTMENTS_AND_AGENCIES", "Government departments and agencies, such as driving licenses, and passport renewal", false },
                    { new Guid("70666376-3200-0000-0000-000000000107"), "GOVERNMENT_AND_NON_PROFIT", "GOVERNMENT_AND_NON_PROFIT_TAX_PAYMENT", "Tax payments, including income and property taxes", false },
                    { new Guid("70666376-3200-0000-0000-000000000108"), "GOVERNMENT_AND_NON_PROFIT", "GOVERNMENT_AND_NON_PROFIT_OTHER_GOVERNMENT_AND_NON_PROFIT", "Other miscellaneous government and non-profit agencies", false },
                    { new Guid("70666376-3200-0000-0000-000000000109"), "TRANSPORTATION", "TRANSPORTATION_BIKES_AND_SCOOTERS", "Bike and scooter rentals", false },
                    { new Guid("70666376-3200-0000-0000-000000000110"), "TRANSPORTATION", "TRANSPORTATION_GAS", "Purchases at a gas station", false },
                    { new Guid("70666376-3200-0000-0000-000000000111"), "TRANSPORTATION", "TRANSPORTATION_PARKING", "Parking fees and expenses", false },
                    { new Guid("70666376-3200-0000-0000-000000000112"), "TRANSPORTATION", "TRANSPORTATION_PUBLIC_TRANSIT", "Public transportation, including rail and train, buses, and metro", false },
                    { new Guid("70666376-3200-0000-0000-000000000113"), "TRANSPORTATION", "TRANSPORTATION_TAXIS_AND_RIDE_SHARES", "Taxi and ride share services", false },
                    { new Guid("70666376-3200-0000-0000-000000000114"), "TRANSPORTATION", "TRANSPORTATION_TOLLS", "Toll expenses", false },
                    { new Guid("70666376-3200-0000-0000-000000000115"), "TRANSPORTATION", "TRANSPORTATION_OTHER_TRANSPORTATION", "Other miscellaneous transportation expenses", false },
                    { new Guid("70666376-3200-0000-0000-000000000116"), "TRAVEL", "TRAVEL_FLIGHTS", "Airline expenses", false },
                    { new Guid("70666376-3200-0000-0000-000000000117"), "TRAVEL", "TRAVEL_LODGING", "Hotels, motels, and hosted accommodation such as Airbnb", false },
                    { new Guid("70666376-3200-0000-0000-000000000118"), "TRAVEL", "TRAVEL_RENTAL_CARS", "Rental cars, charter buses, and trucks", false },
                    { new Guid("70666376-3200-0000-0000-000000000119"), "TRAVEL", "TRAVEL_OTHER_TRAVEL", "Other miscellaneous travel expenses", false },
                    { new Guid("70666376-3200-0000-0000-000000000120"), "RENT_AND_UTILITIES", "RENT_AND_UTILITIES_GAS_AND_ELECTRICITY", "Gas and electricity bills", false },
                    { new Guid("70666376-3200-0000-0000-000000000121"), "RENT_AND_UTILITIES", "RENT_AND_UTILITIES_INTERNET_AND_CABLE", "Internet and cable bills", false },
                    { new Guid("70666376-3200-0000-0000-000000000122"), "RENT_AND_UTILITIES", "RENT_AND_UTILITIES_RENT", "Rent payment", false },
                    { new Guid("70666376-3200-0000-0000-000000000123"), "RENT_AND_UTILITIES", "RENT_AND_UTILITIES_SEWAGE_AND_WASTE_MANAGEMENT", "Sewage and garbage disposal bills", false },
                    { new Guid("70666376-3200-0000-0000-000000000124"), "RENT_AND_UTILITIES", "RENT_AND_UTILITIES_TELEPHONE", "Telephone bills", false },
                    { new Guid("70666376-3200-0000-0000-000000000125"), "RENT_AND_UTILITIES", "RENT_AND_UTILITIES_WATER", "Water bills", false },
                    { new Guid("70666376-3200-0000-0000-000000000126"), "RENT_AND_UTILITIES", "RENT_AND_UTILITIES_OTHER_UTILITIES", "Other miscellaneous utility bills", false },
                    { new Guid("70666376-3200-0000-0000-000000000127"), "OTHER", "OTHER_OTHER", "", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
