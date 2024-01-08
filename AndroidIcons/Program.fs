open System.Xml.Linq

let doc = XDocument()

let ns = XNamespace.Get("http://www.w3.org/2000/svg")
let (!) n = XName.Get(n, ns.NamespaceName)

let svg =
    XElement(
        ! "svg",
        [
            XAttribute("width", "100")
            XAttribute("height", "100")
        ],
        [
            XElement(
                !"def"
                XElement(
                    !"style"
                )
            )
            XElement(
                ! "rect",
                [
                    XAttribute("width", "100")
                    XAttribute("height", "100")
                    XAttribute("rx", "10")
                    XAttribute("ry", "10")
                    XAttribute("fill", "blue")
                ]
            )
            XElement(
                ! "text",
                [
                    XAttribute("x", "50")
                    XAttribute("y", "50")
                    XAttribute("text-anchor", "middle")
                    XAttribute("dominant-baseline", "middle")
                    XAttribute("fill", "white")
                    XAttribute("font-size", "60")
                ],
                [ XText("\uf2fe") ]
            )
        ]

    )

doc.Add(svg)
let str = doc.ToString()
printfn "%s" str
