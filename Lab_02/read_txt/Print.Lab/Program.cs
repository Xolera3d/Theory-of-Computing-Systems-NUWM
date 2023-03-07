const string path = "./read";

if (!Directory.Exists(path))
{
    Console.WriteLine("Directory 'read' not found. Use Volume");
    return;
}

var files = Directory.GetFiles(path).Where(x => Path.GetExtension(x) == ".txt").ToArray();


if (files.Length == 0)
    Console.WriteLine("Пусто");

foreach (var file in files)
{
    Console.WriteLine(Path.GetFileName(file) + ":");
    Console.WriteLine(File.ReadAllText(file));
    Console.WriteLine(Environment.NewLine);
}