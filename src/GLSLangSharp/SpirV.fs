namespace GLSLang.SpirV

open System.IO


module SpirV =
    
    let disassembleStream (stream : Stream) =
        SpirVUtilities.readStream stream

    let disassembleFile (file : string) =
        use stream = new FileStream(file, FileMode.Open)
        SpirVUtilities.readStream stream

    let disassemble (data : byte[]) =
        use stream = new MemoryStream(data)
        SpirVUtilities.readStream stream


    let assembleToStream (stream : Stream) (instructions : seq<Instruction>) =
        SpirVUtilities.writeStream stream (Seq.toList instructions)

    let assembleToFile (file : string) (instructions : seq<Instruction>) =
        use stream = new FileStream(file, FileMode.Create)
        instructions |> assembleToStream stream

    let assemble (instructions : seq<Instruction>) =
        use stream = new MemoryStream()
        instructions |> assembleToStream stream
        stream.ToArray()


    let tryGetResultId (i : Instruction) =
        SpirVUtilities.tryGetId i

    let tryGetResultType (i : Instruction) =
        SpirVUtilities.tryGetResultTypeId i