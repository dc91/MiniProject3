using System.Diagnostics;
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

// Add some assets to every office
officeSWE.AddAsset(new Laptop("Apple", "MacBook", 49999.0m, "SEK", new DateOnly(2021, 12, 22), officeSWE.Name));
officeSWE.AddAsset(new Laptop("Apple", "MacBook", 75990.0m, "SEK", new DateOnly(2022, 5, 11), officeSWE.Name));
officeSWE.AddAsset(new Laptop("Lenovo", "Yoga 20", 18749.0m, "SEK", new DateOnly(2024, 05, 15), officeSWE.Name));
officeSWE.AddAsset(new MobilePhone("Apple", "Ipone", 19900.0m, "SEK", new DateOnly(2024, 08, 13), officeSWE.Name));
officeSWE.AddAsset(new MobilePhone("Samsung", "Galaxy S22", 18999.0m, "SEK", new DateOnly(2023, 11, 08), officeSWE.Name));
officeSWE.AddAsset(new MobilePhone("Samsung", "Galaxy S20", 19999.0m, "SEK", new DateOnly(1999, 10, 17), officeSWE.Name));

officeUSA.AddAsset(new Laptop("Apple", "MacBook", 2999.0m, "USD", new DateOnly(2023, 05, 22), officeUSA.Name));
officeUSA.AddAsset(new Laptop("Lenovo", "Yoga 20", 990.0m, "USD", new DateOnly(2024, 07, 11), officeUSA.Name));
officeUSA.AddAsset(new Laptop("Lenovo", "Yoga 20", 1259.0m, "USD", new DateOnly(2024, 05, 15), officeUSA.Name));
officeUSA.AddAsset(new MobilePhone("Apple", "Ipone", 1990.0m, "USD", new DateOnly(2024, 08, 13), officeUSA.Name));
officeUSA.AddAsset(new MobilePhone("Samsung", "Galaxy S21", 1899.0m, "USD", new DateOnly(2023, 11, 08), officeUSA.Name));
officeUSA.AddAsset(new MobilePhone("Nokia", "3310", 99.0m, "USD", new DateOnly(1999, 10, 17), officeUSA.Name));

officeFIN.AddAsset(new Laptop("Asus", "Vivobook 16", 1599.0m, "EUR", new DateOnly(2023, 05, 22), officeFIN.Name));
officeFIN.AddAsset(new Laptop("Asus", "Vivobook 16", 1499.0m, "EUR", new DateOnly(2024, 07, 11), officeFIN.Name));
officeFIN.AddAsset(new Laptop("Asus", "Vivobook 16", 1879.0m, "EUR", new DateOnly(2024, 05, 15), officeFIN.Name));
officeFIN.AddAsset(new MobilePhone("Nokia", "3310", 1990.0m, "EUR", new DateOnly(2024, 08, 13), officeFIN.Name));
officeFIN.AddAsset(new MobilePhone("Nokia", "3310", 1899.0m, "EUR", new DateOnly(2023, 11, 08), officeFIN.Name));
officeFIN.AddAsset(new MobilePhone("Nokia", "3310", 99.0m, "EUR", new DateOnly(1999, 10, 17), officeFIN.Name));

bool stay = true;


while (stay)
{
    ClearConsole();// Otherwise first letter disapears. some buffer issue to do with inputs like ReadKey()
    PrintMainMenu();

    Console.CursorVisible = false;
    Office? chosenOffice = null;
    ConsoleKey key = Console.ReadKey().Key;

    switch (key)
    {
        case ConsoleKey.D1:
            chosenOffice = ChooseOffice(ref officeList);
            if (chosenOffice != null)
                AddForOffice(ref chosenOffice);
            break;
        case ConsoleKey.D2:
            chosenOffice = ChooseOffice(ref officeList);
            if (chosenOffice != null)
                ListAssetsInOffice(ref chosenOffice);
            break;
        case ConsoleKey.D3:
            ListAllAssets(ref officeList);
            break;
        case ConsoleKey.D4:
            stay = false;
            break;
    }
}


static Office? ChooseOffice(ref List<Office> officeList)
{
    int selectedOffice = 0;
    bool stay = true;

    while (stay)
    {
        Console.Clear();
        Console.WriteLine("Which office?\n");

        for (int i = 0; i < officeList.Count; i++)
        {
            if (i == selectedOffice)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(officeList[i].Name);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(officeList[i].Name);
            }
        }

        ConsoleKey key = Console.ReadKey().Key;

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
    Console.WriteLine($"Adding an Asset for {office.Name} office. Type \'CANCEL\' in any input to cancel\n");
    List<string> types = ["Laptop", "MobilePhone"];

    int selectedType = 0;

    bool stay = true;
    while (stay)
    {
        Console.CursorVisible = false;
        string? assetType = SelectAssetType(types, ref selectedType);
        if (assetType == null) break;
        Console.CursorVisible = true;

        Console.WriteLine();

        string? assetManufacturer = GetInput("Manufacturer: ");
        if (assetManufacturer == null) break;

        string? assetModelName = GetInput("Model name: ");
        if (assetModelName == null) break;

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

        if (assetType == "Laptop")
        {
            office.AddAsset(new Laptop(assetManufacturer, assetModelName, assetPrice, priceCurrency, purchaseDate, office.Name));
            Console.WriteLine("Laptop added");
            Console.ReadKey();
            return;
        }
        else if (assetType == "MobilePhone")
        {
            office.AddAsset(new MobilePhone(assetManufacturer, assetModelName, assetPrice, priceCurrency, purchaseDate, office.Name));
            Console.WriteLine("MobilePhone added");
            Console.ReadKey();
            return;
        }
    }

}

static string? SelectAssetType(List<string> types, ref int selectedType)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Laptop or Mobile phone: ");
        for (int i = 0; i < types.Count; i++)
        {
            if (i == selectedType)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(types[i] + "\t");
                Console.ResetColor();
            }
            else
            {
                Console.Write(types[i] + "\t");
            }
        }

        ConsoleKey key = Console.ReadKey().Key;
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
        string? input = GetInput(prompt);
        if (input == null)
            return -1;

        if (decimal.TryParse(input, out decimal result))
            return result;

        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.Write("Price not recognized. ");
    }
}
static string? GetValidCurrency(string prompt)
{
    while (true)
    {
        string? input = GetInput(prompt)?.ToUpper();
        if (input == null)
            return null;

        if (input == "USD" || input == "EUR" || input == "SEK")
            return input;

        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.Write("Currency not recognized. ");
    }
}
static DateOnly GetValidDate(string prompt)
{
    while (true)
    {
        string? input = GetInput(prompt)?.Trim();
        if (input == null)
            return default;

        if (DateOnly.TryParse(input, out DateOnly result))
            return result;


        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 2);
        Console.Write("Date not recognized. ");
    }
}
static string? GetInput(string prompt)
{

    Console.WriteLine(prompt);
    while (true)
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.SetCursorPosition(0, Console.CursorTop - 2);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine($"Does not accept empty input. {prompt}");
            continue;
        }
        if (input.Trim().Equals("CANCEL"))
            return null;

        return input.Trim();
    }
}

// Task 2 View assets from selected office
static void ListAssetsInOffice(ref Office office)
{
    DateOnly endOfLife = DateOnly.FromDateTime(DateTime.Now).AddMonths(-36);
    List<Asset> sortedAssets = new(office.Assets.OrderBy(a => a.Type));

    bool stay = true;
    while (stay)
    {
        Console.Clear();
        PrintSortOptions(office);
        PrintList(ref sortedAssets, ref endOfLife, ref office);

        ConsoleKey key = Console.ReadKey().Key;

        switch (key)
        {
            case ConsoleKey.D1:
                sortedAssets = [.. sortedAssets.OrderBy(a => a.Type)];
                break;
            case ConsoleKey.D2:
                sortedAssets = [.. sortedAssets.OrderBy(a => a.PurchaseDate)];
                break;
            case ConsoleKey.Escape:
                stay = false;
                continue;
        }
    }
}

static void PrintList(ref List<Asset> sortedAssets, ref DateOnly endOfLife, ref Office office)
{
    Console.WriteLine("| {0, -15} | {1, -20} | {2, -20} | {3, -20} | {4, -10} | {5, -15}",
            "Asset type:", "Manufacturer:", "Model name:", "Local price:", "USD price:", "Date:");
    Console.WriteLine(new string('-', 110));

    foreach (Asset asset in sortedAssets)
    {
        if (asset.PurchaseDate < endOfLife.AddMonths(3))
            Console.ForegroundColor = ConsoleColor.Red;
        else if (asset.PurchaseDate < endOfLife.AddMonths(6))
            Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("| {0, -15} | {1, -20} | {2, -20} | {3, -20} | {4, -10} | {5, -15}",
            asset.Type, asset.Manufacturer, asset.ModelName, Math.Round(asset.Price, 2) + " " + asset.Currency,
            Math.Round(office.ConvertPrice(asset.Price, office.Currency + "USD"),2), asset.PurchaseDate.ToString());
        Console.ResetColor();
    }
}

static void PrintSortOptions(Office office)
{
    Console.WriteLine($"How would you like to sort the assets of {office.Name} office?");
    Console.WriteLine("[1] By Asset Type");
    Console.WriteLine("[2] By Purchase Date");
    Console.WriteLine("[ESC] Go Back");
    Console.WriteLine();
}

// Task 3 View assets from all offices
void ListAllAssets(ref List<Office> officeList)
{
    Console.Clear();
    PrintSortOptionsAll();

    DateOnly endOfLife = DateOnly.FromDateTime(DateTime.Now).AddMonths(-36);
    List<Asset> sortedAssets = officeList.SelectMany(o => o.Assets).ToList();
    sortedAssets = [.. sortedAssets.OrderBy(a => a.Office)];

    bool stay = true;
    while (stay)
    {
        Console.Clear();
        PrintSortOptionsAll();
        PrintListAll(ref sortedAssets, ref endOfLife);

        ConsoleKey key = Console.ReadKey().Key;

        switch (key)
        {
            case ConsoleKey.D1:
                sortedAssets = [.. sortedAssets.OrderBy(a => a.Office)];
                break;
            case ConsoleKey.D2:
                sortedAssets = [.. sortedAssets.OrderBy(a => a.PurchaseDate)];
                break;
            case ConsoleKey.Escape:
                stay = false;
                break;
        }
    }
}

void PrintListAll(ref List<Asset> sortedAssets, ref DateOnly endOfLife)
{
    Console.WriteLine("| {0, -10} | {1, -15} | {2, -15} | {3, -20} | {4, -15} | {5, -10} | {6, -15}",
            "Office:", "Type:", "Manufacturer:", "Model name:", "Local price:", "USD price:", "Date:");
    Console.WriteLine(new string('-', 115));

    foreach (Asset asset in sortedAssets)
    {
        if (asset.PurchaseDate < endOfLife.AddMonths(3))
            Console.ForegroundColor = ConsoleColor.Red;
        else if (asset.PurchaseDate < endOfLife.AddMonths(6))
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
    Console.WriteLine("How would you like to sort the list?");
    Console.WriteLine("[1] By Office");
    Console.WriteLine("[2] By Purchase Date");
    Console.WriteLine("[ESC] Go Back");
    Console.WriteLine();
}

// Rest

static void PrintMainMenu()
{
    Console.WriteLine("Welcome\n\n\n");
    Console.WriteLine("[1] Add an asset to an office");
    Console.WriteLine("[2] View assest per office");
    Console.WriteLine("[3] View all assets");
    Console.WriteLine("[4] quit.");
}

static void ClearConsole()
{
    Process.Start(new ProcessStartInfo
    {
        FileName = "cmd.exe",
        Arguments = "/C cls", // /C runs the command and exits
        UseShellExecute = false,
        RedirectStandardOutput = false
    })?.WaitForExit();
}
