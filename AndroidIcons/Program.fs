open System.Diagnostics
open System.IO
open System.Xml.Linq

let inkscape = @"C:\Program Files\Inkscape\bin\inkscape.exe"
let (</>) a b = Path.Combine(a, b)

let dir d =
    Directory.CreateDirectory(d) |> (_.FullName)

let s = __SOURCE_DIRECTORY__ </> ".." </> "icons"
let targetDirSvg = s </> "svg" |> dir
let targetDirPng = s </> "png" |> dir

let pngExport svgPath pngPath =
    let arguments = $"--export-filename \"{pngPath}\" \"{svgPath}\""

    let processStartInfo =
        ProcessStartInfo(FileName = inkscape, Arguments = arguments, UseShellExecute = false, CreateNoWindow = true)

    let process_ = Process.Start(processStartInfo)
    process_.WaitForExit()

let svgNamespace = XNamespace.Get("http://www.w3.org/2000/svg")

let (!) n =
    XName.Get(n, svgNamespace.NamespaceName)

let rect width height fill =
    string (
        XDocument(
            XElement(
                ! "svg",
                [
                    XAttribute("width", width)
                    XAttribute("height", height)
                ],
                [
                    XElement(
                        ! "rect",
                        [
                            XAttribute("x", 0)
                            XAttribute("y", 0)
                            XAttribute("width", width)
                            XAttribute("height", height)
                            XAttribute("fill", fill)
                        ]
                    )
                ]
            )
        )
    )

let svg icon fontFamily xFix yFix =

    string (
        XDocument(
            XElement(
                ! "svg",
                [
                    XAttribute("width", 512)
                    XAttribute("height", 512)
                ],
                [
                    XElement(
                        ! "rect",
                        [
                            XAttribute("width", 512)
                            XAttribute("height", 512)
                            XAttribute("rx", 100)
                            XAttribute("ry", 100)
                            XAttribute("fill", "#072448")
                        ]
                    )
                    XElement(
                        ! "text",
                        [
                            XAttribute("x", (256 + 5 * xFix).ToString())
                            XAttribute("y", (282 + 5 * yFix).ToString())
                            XAttribute("text-anchor", "middle")
                            XAttribute("dominant-baseline", "middle")
                            XAttribute("fill", "white")
                            XAttribute("font-size", 300)
                            XAttribute("font-family", fontFamily)
                        ],
                        [ XText($"{icon}") ]
                    )
                ]
            )
        )
    )

type IconSet =
    | Solid
    | Brand

let icons = [
    Solid, '\uf0b1', +0, -2, "briefcase"
    Solid, '\uf086', +0, +0, "messages"
    Brand, '\uf1a0', +0, -1, "google"
    Solid, '\uf19c', +0, -4, "bank"
    Solid, '\uf07c', +3, +0, "folder"
    Solid, '\uf073', +0, -1, "calendar"
    Solid, '\uf11b', +0, +0, "games"
    Solid, '\uf544', +0, -5, "robot"
    Solid, '\uf07a', +0, +2, "shopping-cart"
    Solid, '\uf44b', +0, +0, "dumbbell"
    Solid, '\uf3c5', +0, +0, "map"
    Brand, '\uf3cf', +0, +0, "android"
    Solid, '\uf577', +0, +0, "fingerprint"
    Solid, '\uf1b9', +0, +0, "car"
    Solid, '\ue535', +2, +0, "couple"
    Solid, '\uf67b', +0, +0, "meditation"
    Solid, '\uf095', +0, +0, "phone"
    Solid, '\uf0eb', +0, +0, "light-bulb"
    Solid, '\uf268', +0, +0, "chrome"
    Solid, '\uf0ae', +0, +0, "checklist"
    Solid, '\uf4fd', +3, +0, "user-clock"
]

for set, icon, xFix, yFix, name in icons do

    let pathSvg = targetDirSvg </> $"{name}.svg"

    printfn $"Generating %s{pathSvg}"

    let fontFamily =
        match set with
        | Solid -> "Font Awesome 6 Free"
        | Brand -> "Font Awesome 6 Brands"

    let rendered = svg icon fontFamily xFix yFix

    File.WriteAllText(pathSvg, rendered)

    let pathPng = targetDirPng </> $"{name}.png"

    printfn $"Exporting %s{pathPng}"
    pngExport pathSvg pathPng

let background = rect 1080 2412 "#0b2e5b"
File.WriteAllText(targetDirSvg </> "background.svg", background)
pngExport (targetDirSvg </> "background.svg") (targetDirPng </> "background.png")
