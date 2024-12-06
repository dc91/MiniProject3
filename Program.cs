using System.Diagnostics;
using System.Runtime.CompilerServices;
using MiniProject3;


List<KeyValuePair<string, decimal>> currencyQuotes = LiveCurrency.FetchRates();
decimal EURUSDrate = currencyQuotes.FirstOrDefault(q => q.Key == "USD").Value;
decimal USDEURrate = 1 / EURUSDrate;
decimal EURSEKrate = currencyQuotes.FirstOrDefault(q => q.Key == "SEK").Value;
decimal SEKEURrate = 1 / EURSEKrate;
decimal USDSEKrate = EURSEKrate * USDEURrate;
decimal SEKUSDrate = 1 / USDSEKrate;


List<KeyValuePair<string, decimal>> quotesSWE =
[
    new("EURSEK", EURSEKrate),
    new("SEKEUR", SEKEURrate),
    new("USDSEK", USDSEKrate),
    new("SEKUSD", SEKUSDrate),
    new("SEKSEK", 1.0m)
];

List<KeyValuePair<string, decimal>> quotesUSA =
[
    new("EURUSD", EURUSDrate),
    new("USDEUR", USDEURrate),
    new("USDSEK", USDSEKrate),
    new("SEKUSD", SEKUSDrate),
    new("USDUSD", 1.0m)
];

List<KeyValuePair<string, decimal>> quotesFIN =
[
    new("EURUSD", EURUSDrate),
    new("USDEUR", USDEURrate),
    new("EURSEK", EURSEKrate),
    new("SEKEUR", SEKEURrate),
    new("EUREUR", 1.0m)
];

// Add 3 offices
Office officeSWE = new Office("SWE", quotesSWE, "SEK");
Office officeUSA = new Office("USA", quotesUSA, "USD");
Office officeFIN = new Office("FIN", quotesFIN, "EUR");

List<Office> officeList = [officeSWE, officeUSA, officeFIN];

// Add some assets to every office with random dates between 1 year ago and 4 years ago
officeSWE.AddAsset(new Laptop("Apple", "MacBook", 49999.0m, "SEK", GetRandomDate(), officeSWE.Name));
officeSWE.AddAsset(new Laptop("Apple", "MacBook", 75990.0m, "SEK", GetRandomDate(), officeSWE.Name));
officeSWE.AddAsset(new Laptop("Lenovo", "Yoga 20", 18749.0m, "SEK", GetRandomDate(), officeSWE.Name));
officeSWE.AddAsset(new MobilePhone("Apple", "Ipone", 19900.0m, "SEK", GetRandomDate(), officeSWE.Name));
officeSWE.AddAsset(new MobilePhone("Samsung", "Galaxy S22", 18999.0m, "SEK", GetRandomDate(), officeSWE.Name));
officeSWE.AddAsset(new MobilePhone("Samsung", "Galaxy S20", 19999.0m, "SEK", GetRandomDate(), officeSWE.Name));

officeUSA.AddAsset(new Laptop("Apple", "MacBook", 2999.0m, "USD", GetRandomDate(), officeUSA.Name));
officeUSA.AddAsset(new Laptop("Lenovo", "Yoga 20", 990.0m, "USD", GetRandomDate(), officeUSA.Name));
officeUSA.AddAsset(new Laptop("Lenovo", "Yoga 20", 1259.0m, "USD", GetRandomDate(), officeUSA.Name));
officeUSA.AddAsset(new MobilePhone("Apple", "Ipone", 1990.0m, "USD", GetRandomDate(), officeUSA.Name));
officeUSA.AddAsset(new MobilePhone("Samsung", "Galaxy S21", 1899.0m, "USD", GetRandomDate(), officeUSA.Name));
officeUSA.AddAsset(new MobilePhone("Nokia", "3310", 99.0m, "USD", GetRandomDate(), officeUSA.Name));

officeFIN.AddAsset(new Laptop("Asus", "Vivobook 16", 1599.0m, "EUR", GetRandomDate(), officeFIN.Name));
officeFIN.AddAsset(new Laptop("Asus", "Vivobook 16", 1499.0m, "EUR", GetRandomDate(), officeFIN.Name));
officeFIN.AddAsset(new Laptop("Asus", "Vivobook 16", 1879.0m, "EUR", GetRandomDate(), officeFIN.Name));
officeFIN.AddAsset(new MobilePhone("Nokia", "3310", 1990.0m, "EUR", GetRandomDate(), officeFIN.Name));
officeFIN.AddAsset(new MobilePhone("Nokia", "3310", 1899.0m, "EUR", GetRandomDate(), officeFIN.Name));
officeFIN.AddAsset(new MobilePhone("Nokia", "3310", 99.0m, "EUR", GetRandomDate(), officeFIN.Name));


// Main Menu
bool stay = true;
while (stay)
{
    Console.SetCursorPosition(0, 0);
    ClearLines();
    PrintMainMenu();

    Console.CursorVisible = false;
    Office? chosenOffice = null;

    ConsoleKey key = Console.ReadKey(intercept: true).Key;
    switch (key)
    {
        case ConsoleKey.D1:
            chosenOffice = ChooseOffice(ref officeList);
            if (chosenOffice != null)
                AddForOffice(ref chosenOffice);
            break;
        case ConsoleKey.D2:
            ListAssetsInOffice(ref officeList);
            break;
        case ConsoleKey.D3:
            ListAllAssets(ref officeList);
            break;
        case ConsoleKey.Escape:
            stay = false;
            break;
        default:
            break;
    }
}



static Office? ChooseOffice(ref List<Office> officeList)
{
    int selectedOffice = 0;
    bool stay = true;

    while (stay)
    {
        Console.SetCursorPosition(0, 0);
        ClearLines();
        Console.WriteLine("Which office?\n");

        for (int i = 0; i < officeList.Count; i++)
        {
            if (i == selectedOffice)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("> " + officeList[i].Name);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("  " + officeList[i].Name);
            }
        }

        ConsoleKey key = Console.ReadKey(intercept: true).Key;

        switch (key)
        {
            case ConsoleKey.DownArrow:
                if (selectedOffice < officeList.Count - 1)
                    selectedOffice++;
                break;
            case ConsoleKey.UpArrow:
                if (selectedOffice > 0)
                    selectedOffice--;
                break;
            case ConsoleKey.Enter:
                return officeList[selectedOffice];
            case ConsoleKey.Escape:
                stay = false;
                break;
        }
    }
    return null;
}

static void AddForOffice(ref Office office)
{
    Console.Clear();
    List<string> types = ["Laptop", "MobilePhone"];

    int selectedType = 0;

    bool stay = true;
    while (stay)
    {
        Console.CursorVisible = false;
        string? assetType = SelectAssetType(types, ref selectedType, office);
        if (assetType == null) break;
        

        Console.WriteLine();
        Console.CursorVisible = true;
        string assetManufacturer = GetInput("Manufacturer: ");
        if (assetManufacturer == string.Empty) break;

        string assetModelName = GetInput("Model name: ");
        if (assetModelName == string.Empty) break;

        decimal assetPrice = GetValidDecimal("Price (Expecting numbers with eventual comma to signify decimals): ");
        if (assetPrice == -1)
            break;

        string? priceCurrency = GetValidCurrency("Currency (USD, EUR or SEK): ");
        if (priceCurrency == null)
            break;

        DateOnly purchaseDate = GetValidDate("Purchase Date (yyyy-mm-dd): ");
        if (purchaseDate == default)
            break;

        if (priceCurrency != office.Currency)
        {
            assetPrice = office.ConvertPrice(assetPrice, priceCurrency + office.Currency);
            priceCurrency = office.Currency;
        }
        Console.CursorVisible = false;
        if (assetType == "Laptop")
        {
            office.AddAsset(new Laptop(assetManufacturer, assetModelName, assetPrice, priceCurrency, purchaseDate, office.Name));
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nLaptop added. Press any key to go back to Main Menu");
            Console.ResetColor();
            Console.ReadKey(intercept: true);
            return;
        }
        else if (assetType == "MobilePhone")
        {
            office.AddAsset(new MobilePhone(assetManufacturer, assetModelName, assetPrice, priceCurrency, purchaseDate, office.Name));
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nMobilePhone added. Press any key to go back to Main Menu");
            Console.ResetColor();
            Console.ReadKey(intercept: true);
            return;
        }
    }

}

// Just to make user select instead of typing the type. One less possible user misstake, hopefully
static string? SelectAssetType(List<string> types, ref int selectedType, Office office)
{
    while (true)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"Adding an Asset for {office.Name} office. \nPress ESC to cancel.\n");
        Console.WriteLine("Laptop or Mobile phone: ");
        for (int i = 0; i < types.Count; i++)
        {
            if (i == selectedType)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("> " + types[i] + "\t");
                Console.ResetColor();
            }
            else
            {
                Console.Write("  " + types[i] + "\t");
            }
        }

        ConsoleKey key = Console.ReadKey(intercept: true).Key;
        switch (key)
        {
            case ConsoleKey.RightArrow:
                if (selectedType < types.Count - 1)
                    selectedType++;
                break;
            case ConsoleKey.LeftArrow:
                if (selectedType > 0)
                    selectedType--;
                break;
            case ConsoleKey.Enter:
                return types[selectedType];
            case ConsoleKey.Escape:
                return null;
        }
    }
}


// Input validation
static decimal GetValidDecimal(string prompt)
{
    while (true)
    {
        string input = GetInput(prompt);
        if (input == string.Empty)
            return -1;

        if (decimal.TryParse(input, out decimal result))
            return result;

        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Price not recognized. ");
        Console.ResetColor();
    }
}
static string? GetValidCurrency(string prompt)
{
    while (true)
    {
        string input = GetInput(prompt).ToUpper();
        if (input == string.Empty)
            return null;

        if (input == "USD" || input == "EUR" || input == "SEK")
            return input;

        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Currency not recognized. ");
        Console.ResetColor();
    }
}
static DateOnly GetValidDate(string prompt)
{
    while (true)
    {
        string input = GetInput(prompt).Trim();
        if (input == string.Empty)
            return default;

        if (DateOnly.TryParse(input, out DateOnly result))
            return result;


        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Date not recognized. ");
        Console.ResetColor();
    }
}
static string GetInput(string prompt)
{
    string input = string.Empty;
    Console.WriteLine(prompt);
    while (true)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); // intercept: true to prevent the key from being displayed

        if (keyInfo.Key == ConsoleKey.Escape)
            return string.Empty;
        else if (keyInfo.Key == ConsoleKey.Enter)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Does not accept empty input. ");
                Console.ResetColor();
                Console.WriteLine(prompt);
                continue;
            }
            else
            {
                Console.WriteLine();
                return input;
            }
        }
        else if (keyInfo.Key == ConsoleKey.Backspace)
        {
            if (input.Length > 0)
            {
                input = input.Substring(0, input.Length - 1);

                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                Console.Write(' ');
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }
        else if (!char.IsControl(keyInfo.KeyChar) && keyInfo.KeyChar != '§')
        {
            Console.Write(keyInfo.KeyChar); // Display the pressed key character
            input += keyInfo.KeyChar;  // Append to input
        }

    }
}

// Task 2 View assets from selected office
void ListAssetsInOffice(ref List<Office> officeList)
{
    Office office = officeSWE;
    int sortingOption = 1;
    bool filtered = false;
    bool stay = true;

    Console.Clear();
    PrintSortOptions(office);
    int tableStart = Console.CursorTop;

    while (stay)
    {   // Sort & filter settings
        List<Asset> sortedAssets = sortingOption switch
        {
            1 => new List<Asset>(office.Assets.OrderBy(a => a.Type)),
            2 => new List<Asset>(office.Assets.OrderBy(a => a.PurchaseDate)),
            _ => new List<Asset>(office.Assets)
        };
        if (filtered)
            sortedAssets = sortedAssets.Where(a => a.EndOfLifeStatus != "Normal").ToList();

        Console.SetCursorPosition(0, tableStart);// instead of clearing full screen
        ClearLines();
        PrintList(ref sortedAssets, ref office);

        ConsoleKey key = Console.ReadKey(intercept: true).Key;

        switch (key)
        {
            case ConsoleKey.D1:
                if (sortingOption == 1)
                    break;
                sortingOption = 1;
                break;
            case ConsoleKey.D2:
                if (sortingOption == 2)
                    break;
                sortingOption = 2;
                break;
            case ConsoleKey.U:
                if (office == officeUSA)
                    break;
                office = officeUSA;
                break;
            case ConsoleKey.S:
                if (office == officeSWE)
                    break;
                office = officeSWE;
                break;
            case ConsoleKey.F:
                if (office == officeFIN)
                    break;
                office = officeFIN;
                break;
            case ConsoleKey.N:
                filtered = !filtered; // Toggle filtering
                break;
            case ConsoleKey.Escape:
                stay = false;
                break;
            default:
                break;
        }
    }
}

static void PrintList(ref List<Asset> sortedAssets, ref Office office)
{
    Console.WriteLine("| {0, -15} | {1, -20} | {2, -20} | {3, -20} | {4, -10} | {5, -15}",
            "Asset type:", "Manufacturer:", "Model name:", "Local price:", "USD price:", "Date:");
    Console.WriteLine(new string('-', 110));

    foreach (Asset asset in sortedAssets)
    {
        if (asset.EndOfLifeStatus == "Past End of Life")
            Console.ForegroundColor = ConsoleColor.DarkGray;
        else if (asset.EndOfLifeStatus == "3 Months Left")
            Console.ForegroundColor = ConsoleColor.Red;
        else if (asset.EndOfLifeStatus == "6 Months Left")
            Console.ForegroundColor = ConsoleColor.Yellow;
        
        Console.WriteLine("| {0, -15} | {1, -20} | {2, -20} | {3, -20} | {4, -10} | {5, -15}",
            asset.Type, asset.Manufacturer, asset.ModelName, Math.Round(asset.Price, 2) + " " + asset.Currency,
            Math.Round(office.ConvertPrice(asset.Price, office.Currency + "USD"),2), asset.PurchaseDate.ToString());
        Console.ResetColor();
    }
}

static void PrintSortOptions(Office office)
{
    Console.WriteLine($"Sort the assets of {office.Name} office?" + "\t\t\t" + "Change Office?" + "\t\t\t" + "Color Code:");
    Console.Write("[1] By Asset Type" + "\t\t\t\t" + "[U] USA Office" + "\t\t\t");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("6 months until end-of-life");
    Console.ResetColor();
    Console.Write("[2] By Purchase Date" + "\t\t\t\t" + "[S] SWE Office" + "\t\t\t");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("3 months until end-of-life");
    Console.ResetColor();
    Console.Write("[N] Toggle Danger Zone" + "\t\t\t\t" + "[F] FIN Office" + "\t\t\t");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("Past end-of-life");
    Console.ResetColor();
    Console.WriteLine("\n[ESC] Go Back");
    Console.WriteLine();
}

// Task 3 View assets from all offices
void ListAllAssets(ref List<Office> officeList)
{
    int sortingOption = 1;
    bool filtered = false;
    bool stay = true;

    Console.Clear();
    PrintSortOptionsAll();
    int tableStart = Console.CursorTop;

    while (stay)
    {
        List<Asset> sortedAssets = sortingOption switch
        {
            1 => [.. officeList.SelectMany(o => o.Assets).OrderBy(a => a.Office)],
            2 => [.. officeList.SelectMany(o => o.Assets).OrderBy(a => a.PurchaseDate)],
            _ => officeList.SelectMany(o => o.Assets).ToList()
        };

        if (filtered)
            sortedAssets = sortedAssets.Where(a => a.EndOfLifeStatus != "Normal").ToList();

        Console.SetCursorPosition(0, tableStart);// instead of clearing full screen
        ClearLines();
        PrintListAll(ref sortedAssets);

        ConsoleKey key = Console.ReadKey(intercept: true).Key;

        switch (key)
        {
            case ConsoleKey.D1:
                if (sortingOption == 1)
                    break;
                sortingOption = 1;
                break;
            case ConsoleKey.D2:
                if (sortingOption == 2)
                    break;
                sortingOption = 2;
                break;
            case ConsoleKey.N:
                filtered = !filtered;
                break;
            case ConsoleKey.Escape:
                stay = false;
                break;
            default:
                break;
        }
    }
}

void PrintListAll(ref List<Asset> sortedAssets)
{
    Console.WriteLine("| {0, -10} | {1, -15} | {2, -15} | {3, -20} | {4, -15} | {5, -10} | {6, -15}",
            "Office:", "Type:", "Manufacturer:", "Model name:", "Local price:", "USD price:", "Date:");
    Console.WriteLine(new string('-', 115));

    foreach (Asset asset in sortedAssets)
    {
        if (asset.EndOfLifeStatus == "Past End of Life")
            Console.ForegroundColor = ConsoleColor.DarkGray;
        else if (asset.EndOfLifeStatus == "3 Months Left")
            Console.ForegroundColor = ConsoleColor.Red;
        else if (asset.EndOfLifeStatus == "6 Months Left")
            Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("| {0, -10} | {1, -15} | {2, -15} | {3, -20} | {4, -15} | {5, -10} | {6, -15}",
            asset.Office, asset.Type, asset.Manufacturer, asset.ModelName, Math.Round(asset.Price, 2) + " " + asset.Currency,
            Math.Round(officeUSA.ConvertPrice(asset.Price, asset.Currency + "USD"), 2),
            asset.PurchaseDate.ToString());
        Console.ResetColor();
    }
}

static void PrintSortOptionsAll()
{
    Console.WriteLine("How would you like to sort the list?" + "\t\t\t" + "Color Code:");
    Console.Write("[1] By Office" + "\t\t\t\t\t\t");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("6 months until end-of-life");
    Console.ResetColor();
    Console.Write("[2] By Purchase Date" + "\t\t\t\t\t");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("3 months until end-of-life");
    Console.ResetColor();
    Console.Write("[N] Toggle Danger Zone" + "\t\t\t\t\t");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("Past end-of-life");
    Console.ResetColor();
    Console.WriteLine("\n[ESC] Go Back");
    Console.WriteLine();
}

// Rest

static void PrintMainMenu()
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Welcome to Your Asset Tracker\n");
    Console.ResetColor();
    Console.WriteLine("[1] Add an Asset to an Office");
    Console.WriteLine("[2] View Assests per Office");
    Console.WriteLine("[3] View All Assets");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("\n[ESC] quit.");
    Console.ResetColor();
    
}

static void ClearLines()
{
    var currPos = Console.GetCursorPosition();
    for (int i = 0; i < Console.WindowHeight - currPos.Top - 1; i++)
    {
        Console.WriteLine(new string(' ', Console.WindowWidth));
    }
    Console.SetCursorPosition(currPos.Left, currPos.Top);
}

static DateOnly GetRandomDate()
{
    DateOnly startDate = DateOnly.FromDateTime(DateTime.Now).AddMonths(-48);
    DateOnly endDate = DateOnly.FromDateTime(DateTime.Now).AddMonths(-12);
    Random random = new Random();
    int range = (endDate.ToDateTime(new TimeOnly(0, 0)) - startDate.ToDateTime(new TimeOnly(0, 0))).Days;
    return startDate.AddDays(random.Next(range));
}