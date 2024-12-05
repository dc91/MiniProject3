using System.Collections.Generic;
using MiniProject3;

// Add 3 offices
Office officeSWE = new Office("SWE", 10.93m, "SEK");
Office officeUSA = new Office("USA", 1.0m, "USD");
Office officeFIN = new Office("FIN", 0.95m, "EUR");

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
	Console.Clear();
	MainMenuPrint();
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
        Console.WriteLine("Which office?");

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
    Console.WriteLine("Laptop or MobilePhone: ");
    string assetType = Console.ReadLine();
    Console.WriteLine("Manufacturer: ");
    string assetManufacturer = Console.ReadLine();
    Console.WriteLine("Model name: ");
    string assetModelName = Console.ReadLine();
    Console.WriteLine("Price: ");
    decimal assetPrice = Convert.ToDecimal(Console.ReadLine());
    Console.WriteLine("Currency: ");
    string priceCurrency = Console.ReadLine();
    Console.WriteLine("Purchase date: ");
    DateOnly purchaseDate = DateOnly.Parse(Console.ReadLine());

    if (priceCurrency == "usd")
    {
        assetPrice = office.ConvertPriceFromUSD(assetPrice);
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

static void PrintList(ref List<Asset> sortedAssets, ref DateOnly endOfLife)
{
    Console.WriteLine("| {0, -15} | {1, -20} | {2, -20} | {3, -10} | {4, -10} | {5, -15}",
            "Asset type:", "Manufacturer:", "Model name:", "Price:", "Currency:", "Date:");
    Console.WriteLine(new string('-', 95));

    foreach (Asset asset in sortedAssets)
    {
        if (asset.PurchaseDate < endOfLife.AddMonths(3))
            Console.ForegroundColor = ConsoleColor.Red;
        else if (asset.PurchaseDate < endOfLife.AddMonths(6))
            Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("| {0, -15} | {1, -20} | {2, -20} | {3, -10} | {4, -10} | {5, -15}",
            asset.Type, asset.Manufacturer, asset.ModelName, asset.Price, asset.Currency, asset.PurchaseDate.ToString());
        Console.ResetColor();
    }
}
static void PrintListAll(ref List<Asset> sortedAssets, ref DateOnly endOfLife)
{
    Console.WriteLine("| {0, -10} | {1, -20} | {2, -20} | {3, -10} | {4, -10} | {5, -15}",
            "Office:", "Manufacturer:", "Model name:", "Price:", "Currency:", "Date:");
    Console.WriteLine(new string('-', 95));

    foreach (Asset asset in sortedAssets)
    {
        if (asset.PurchaseDate < endOfLife.AddMonths(3))
            Console.ForegroundColor = ConsoleColor.Red;
        else if (asset.PurchaseDate < endOfLife.AddMonths(6))
            Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("| {0, -10} | {1, -20} | {2, -20} | {3, -10} | {4, -10} | {5, -15}",
            asset.Office, asset.Manufacturer, asset.ModelName, asset.Price, asset.Currency, asset.PurchaseDate.ToString());

        Console.ResetColor();
    }
}
static void PrintSortOptions()
{
    Console.WriteLine("How would you like to sort the list?");
    Console.WriteLine("[1] By Asset Type");
    Console.WriteLine("[2] By Purchase Date");
    Console.WriteLine("[ESC] Go Back");
}
static void PrintSortOptionsAll()
{
    Console.WriteLine("How would you like to sort the list?");
    Console.WriteLine("[1] By Office");
    Console.WriteLine("[2] By Purchase Date");
    Console.WriteLine("[ESC] Go Back");
}


static void ListAssetsInOffice(ref Office office)
{
    DateOnly endOfLife = DateOnly.FromDateTime(DateTime.Now).AddMonths(-36);
    List<Asset> sortedAssets = new(office.Assets.OrderBy(a => a.Type));

    bool stay = true;
    while (stay)
    {
        Console.Clear();
        PrintSortOptions();
        PrintList(ref sortedAssets, ref endOfLife);

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

static void ListAllAssets(ref List<Office> officeList)
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
                continue;
        }
    }
}


static void MainMenuPrint()
{
	Console.WriteLine("Welcome\n\n\n");
    Console.WriteLine("[1] Add an asset to an office");
    Console.WriteLine("[2] View assest per office");
    Console.WriteLine("[3] View all assets");
    Console.WriteLine("[4] quit.");
}