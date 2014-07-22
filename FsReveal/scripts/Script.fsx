#r @"..\..\..\FsReveal\src\FsReveal\bin\FsReveal.dll"

open System.IO

let outDir = @"g:\output"

let fsxFile = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\presentations\FsReveal.fsx")
FsReveal.generateOutputFromScriptFile outDir "test-fsx.html" fsxFile

let mdFile = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\presentations\FsReveal.md")
FsReveal.generateOutputFromMarkdownFile outDir "test-md.html" mdFile
