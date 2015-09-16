#load "SpirVCore.fs"

open System
open System.Text
open System.IO
open GLSLang.SpirV

do Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

let sb = StringBuilder()
let printfn fmt =
    Printf.kprintf (fun str -> sb.AppendLine str |> ignore; System.Console.WriteLine str) fmt

type Argument =
    | ResultType
    | ResultId

    | Arg of string * Type

type InstructionPrototype = { opCode : OpCode; args : list<Argument> }

let prototypes =
    [
        // Misc
        { opCode = OpCode.Nop; args = [] }
        { opCode = OpCode.Undef; args = [ResultType; ResultId] }

        // Debug
        { opCode = OpCode.Source; args = [Arg("language", typeof<SourceLanguage>); Arg("version", typeof<int>)] }
        { opCode = OpCode.SourceExtension; args = [Arg("extension", typeof<string>)] }
        { opCode = OpCode.Name; args = [Arg("target", typeof<int>); Arg("name", typeof<string>)] }
        { opCode = OpCode.MemberName; args = [Arg("_type", typeof<int>); Arg("mem", typeof<int>); Arg("name", typeof<string>)] }
        { opCode = OpCode.String; args = [ResultId; Arg("value", typeof<string>)] }
        { opCode = OpCode.Line; args = [Arg("target", typeof<int>); Arg("file", typeof<int>); Arg("line", typeof<int>); Arg("col", typeof<int>)] }

        // Annotations
        { opCode = OpCode.Decorate; args = [Arg("target", typeof<int>); Arg("decoration", typeof<Decoration>); Arg("args", typeof<int[]>)] }
        { opCode = OpCode.MemberDecorate; args = [Arg("structureType", typeof<int>); Arg("mem", typeof<int>); Arg("decoration", typeof<Decoration>); Arg("args", typeof<int[]>)] }
        { opCode = OpCode.DecorationGroup; args = [ResultId] }
        { opCode = OpCode.GroupDecorate; args = [Arg("decorationGroup", typeof<int>); Arg("targets", typeof<int[]>)] }
        { opCode = OpCode.GroupMemberDecorate; args = [Arg("decorationGroup", typeof<int>); Arg("idLiteralPairs", typeof<int[]>)] }

        // Extensions
        { opCode = OpCode.Extension; args = [Arg("extName", typeof<string>)] }
        { opCode = OpCode.ExtInstImport; args = [ResultId; Arg("name", typeof<string>)] }
        { opCode = OpCode.ExtInst; args = [ResultType; ResultId; Arg("set", typeof<int>); Arg("instruction", typeof<int>); Arg("args", typeof<int[]>)] }

        // Modes
        { opCode = OpCode.MemoryModel; args = [Arg("addressingModel", typeof<AddressingModel>); Arg("memoryModel", typeof<MemoryModel>)] }
        { opCode = OpCode.EntryPoint; args = [Arg("execModel", typeof<ExecutionModel>); Arg("id", typeof<int>); Arg("name", typeof<string>)] }
        { opCode = OpCode.ExecutionMode; args = [Arg("entryPoint", typeof<int>); Arg("mode", typeof<ExecutionMode>); Arg("args", typeof<int[]>)] }
        { opCode = OpCode.Capability; args = [Arg("cap", typeof<Capability>)] }

        // Type-Declarations
        { opCode = OpCode.TypeVoid; args = [ResultId] }
        { opCode = OpCode.TypeBool; args = [ResultId] }
        { opCode = OpCode.TypeInt; args = [ResultId; Arg("width", typeof<int>); Arg("signed", typeof<bool>)] }
        { opCode = OpCode.TypeFloat; args = [ResultId; Arg("width", typeof<int>)] }
        { opCode = OpCode.TypeVector; args = [ResultId; Arg("compType", typeof<int>); Arg("compCount", typeof<int>)] }
        { opCode = OpCode.TypeMatrix; args = [ResultId; Arg("colType", typeof<int>); Arg("colCount", typeof<int>)] }
        { opCode = OpCode.TypeImage; args = [ResultId; Arg("sampledType", typeof<int>); Arg("dim", typeof<int>); Arg("depth", typeof<int>); Arg("arrayed", typeof<bool>); Arg("ms", typeof<bool>); Arg("sampled", typeof<SampleMode>); Arg("format", typeof<ImageFormat>); Arg("access", typeof<int[]>)] }
        { opCode = OpCode.TypeSampler; args = [ResultId] }
        { opCode = OpCode.TypeSampledImage; args = [ResultId; Arg("imageType", typeof<int>)] }
        { opCode = OpCode.TypeArray; args = [ResultId; Arg("elemType", typeof<int>); Arg("length", typeof<int>)] }
        { opCode = OpCode.TypeRuntimeArray; args = [ResultId; Arg("elemType", typeof<int>)] }
        { opCode = OpCode.TypeStruct; args = [ResultId; Arg("fieldTypes", typeof<int[]>)] }
        { opCode = OpCode.TypeOpaque; args = [ResultId; Arg("name", typeof<string>)] }
        { opCode = OpCode.TypePointer; args = [ResultId; Arg("sClass", typeof<StorageClass>); Arg("_type", typeof<int>)] }
        { opCode = OpCode.TypeFunction; args = [ResultId; Arg("retType", typeof<int>); Arg("argTypes", typeof<int[]>)] }
        { opCode = OpCode.TypeEvent; args = [ResultId] }
        { opCode = OpCode.TypeDeviceEvent; args = [ResultId] }
        { opCode = OpCode.TypeReserveId; args = [ResultId] }
        { opCode = OpCode.TypeQueue; args = [ResultId] }
        { opCode = OpCode.TypePipe; args = [ResultId; Arg("_type", typeof<int>); Arg("access", typeof<AccessQualifier>)] }

        // Constants
        { opCode = OpCode.ConstantTrue; args = [ResultType; ResultId] }
        { opCode = OpCode.ConstantFalse; args = [ResultType; ResultId] }
        { opCode = OpCode.Constant; args = [ResultType; ResultId; Arg("values", typeof<int[]>)] }
        { opCode = OpCode.ConstantComposite; args = [ResultType; ResultId; Arg("constituents", typeof<int[]>)] }
        { opCode = OpCode.ConstantSampler; args = [ResultType; ResultId; Arg("addressingMode", typeof<SamplerAddressingMode>); Arg("normalized", typeof<bool>); Arg("filter", typeof<SamplerFilterMode>)] }
        { opCode = OpCode.ConstantNull; args = [ResultType; ResultId] }
            
        // SpecConstants
        { opCode = OpCode.SpecConstantTrue; args = [ResultType; ResultId] }
        { opCode = OpCode.SpecConstantFalse; args = [ResultType; ResultId] }
        { opCode = OpCode.SpecConstant; args = [ResultType; ResultId; Arg("values", typeof<int[]>)] }
        { opCode = OpCode.SpecConstantComposite; args = [ResultType; ResultId; Arg("constituents", typeof<int[]>)] }
        { opCode = OpCode.SpecConstantOp; args = [ResultType; ResultId; Arg("opCode", typeof<OpCode>); Arg("operands", typeof<int[]>)] }

        // Memory
        { opCode = OpCode.Variable; args = [ResultType; ResultId; Arg("storageClas", typeof<StorageClass>); Arg("initializers", typeof<int[]>)] }
        { opCode = OpCode.ImageTexelPointer; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<Dim>); Arg("sample", typeof<int>)] }
        { opCode = OpCode.Load; args = [ResultType; ResultId; Arg("pointer", typeof<int>); Arg("memoryAccess", typeof<int[]>)] }
        { opCode = OpCode.Store; args = [Arg("pointer", typeof<int>); Arg("ob", typeof<int>); Arg("memoryAccess", typeof<int[]>)] }
        { opCode = OpCode.CopyMemory; args = [Arg("target", typeof<int>); Arg("source", typeof<int>); Arg("memoryAccess", typeof<int[]>)] }
        { opCode = OpCode.CopyMemorySized; args = [Arg("target", typeof<int>); Arg("source", typeof<int>); Arg("size", typeof<int>); Arg("memoryAccess", typeof<int[]>)] }
        { opCode = OpCode.AccessChain; args = [ResultType; ResultId; Arg("_base", typeof<int>); Arg("indices", typeof<int[]>)] }
        { opCode = OpCode.InBoundsAccessChain; args = [ResultType; ResultId; Arg("_base", typeof<int>); Arg("indices", typeof<int[]>)] }
        { opCode = OpCode.PtrAccessChain; args = [ResultType; ResultId; Arg("_base", typeof<int>); Arg("element", typeof<int>); Arg("indices", typeof<int[]>)] }
        { opCode = OpCode.ArrayLength; args = [ResultType; ResultId; Arg("structure", typeof<int>); Arg("arrMember", typeof<int>)] }
        { opCode = OpCode.GenericPtrMemSemantics; args = [ResultType; ResultId; Arg("ptr", typeof<int>)] }

        // Functions
        { opCode = OpCode.Function; args = [ResultType; ResultId; Arg("ctrl", typeof<FunctionControl>); Arg("_type", typeof<int>)] }
        { opCode = OpCode.FunctionParameter; args = [ResultType; ResultId] }
        { opCode = OpCode.FunctionEnd; args = [] }
        { opCode = OpCode.FunctionCall; args = [ResultType; ResultId; Arg("f", typeof<int>); Arg("args", typeof<int[]>)] }

        // Images
        { opCode = OpCode.SampledImage; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("sampler", typeof<int>)] }
        { opCode = OpCode.ImageSampleImplicitLod; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageSampleExplicitLod; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageSampleDrefImplicitLod; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("dref", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageSampleDrefExplicitLod; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("dref", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageSampleProjImplicitLod; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageSampleProjExplicitLod; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageSampleProjDrefImplicitLod; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("dref", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageSampleProjDrefExplicitLod; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("dref", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageFetch; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageGather; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("comp", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageDrefGather; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("dref", typeof<int>); Arg("operands", typeof<int[]>)] }
        { opCode = OpCode.ImageRead; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>) ] }
        { opCode = OpCode.ImageWrite; args = [Arg("image", typeof<int>); Arg("coordinate", typeof<int>); Arg("texel", typeof<int>) ] }
        { opCode = OpCode.ImageQueryDim; args = [ResultType; ResultId; Arg("image", typeof<int>) ] }
        { opCode = OpCode.ImageQueryFormat; args = [ResultType; ResultId; Arg("image", typeof<int>) ] }
        { opCode = OpCode.ImageQueryOrder; args = [ResultType; ResultId; Arg("image", typeof<int>) ] }
        { opCode = OpCode.ImageQuerySizeLod; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("level", typeof<int>) ] }
        { opCode = OpCode.ImageQuerySize; args = [ResultType; ResultId; Arg("image", typeof<int>) ] }
        { opCode = OpCode.ImageQueryLod; args = [ResultType; ResultId; Arg("image", typeof<int>); Arg("coordinate", typeof<int>) ] }
        { opCode = OpCode.ImageQueryLevels; args = [ResultType; ResultId; Arg("image", typeof<int>) ] }
        { opCode = OpCode.ImageQuerySamples; args = [ResultType; ResultId; Arg("image", typeof<int>) ] }

        // Conversion
        { opCode = OpCode.ConvertFToU; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.ConvertFToS; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.ConvertSToF; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.ConvertUToF; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.UConvert; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.SConvert; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.FConvert; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.QuantizeToF16; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.ConvertPtrToU; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.SatConvertSToU; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.SatConvertUToS; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.ConvertUToPtr; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.PtrCastToGeneric; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.GenericCastToPtr; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.GenericCastToPtrExplicit; args = [ResultType; ResultId; Arg("value", typeof<int>); Arg("_class", typeof<StorageClass>) ] }
        { opCode = OpCode.Bitcast; args = [ResultType; ResultId; Arg("operand", typeof<int>) ] }

        // Composite instructions
        { opCode = OpCode.VectorExtractDynamic; args = [ResultType; ResultId; Arg("vector", typeof<int>); Arg("index", typeof<int>) ] }
        { opCode = OpCode.VectorInsertDynamic; args = [ResultType; ResultId; Arg("vector", typeof<int>); Arg("comp", typeof<int>); Arg("index", typeof<int>) ] }
        { opCode = OpCode.VectorShuffle; args = [ResultType; ResultId; Arg("vec1", typeof<int>); Arg("vec2", typeof<int>); Arg("components", typeof<int[]>) ] }
        { opCode = OpCode.CompositeConstruct; args = [ResultType; ResultId; Arg("components", typeof<int[]>) ] }
        { opCode = OpCode.CompositeExtract; args = [ResultType; ResultId; Arg("composite", typeof<int>); Arg("components", typeof<int[]>) ] }
        { opCode = OpCode.CompositeInsert; args = [ResultType; ResultId; Arg("value", typeof<int>); Arg("composite", typeof<int>); Arg("components", typeof<int[]>) ] }
        { opCode = OpCode.CopyObject; args = [ResultType; ResultId; Arg("operand", typeof<int>) ] }
        { opCode = OpCode.Transpose; args = [ResultType; ResultId; Arg("matrix", typeof<int>) ] }

        // Arithmetic instructions
        { opCode = OpCode.SNegate; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }
        { opCode = OpCode.FNegate; args = [ResultType; ResultId; Arg("value", typeof<int>) ] }

        { opCode = OpCode.IAdd; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FAdd; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.ISub; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FSub; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.IMul; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FMul; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.UDiv; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.SDiv; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FDiv; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.UMod; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.SRem; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.SMod; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FRem; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FMod; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.VectorTimesScalar; args = [ResultType; ResultId; Arg("v", typeof<int>); Arg("s", typeof<int>) ] }
        { opCode = OpCode.MatrixTimesScalar; args = [ResultType; ResultId; Arg("m", typeof<int>); Arg("s", typeof<int>) ] }
        { opCode = OpCode.VectorTimesMatrix; args = [ResultType; ResultId; Arg("v", typeof<int>); Arg("m", typeof<int>) ] }
        { opCode = OpCode.MatrixTimesVector; args = [ResultType; ResultId; Arg("m", typeof<int>); Arg("v", typeof<int>) ] }
        { opCode = OpCode.MatrixTimesMatrix; args = [ResultType; ResultId; Arg("m0", typeof<int>); Arg("m1", typeof<int>) ] }
        { opCode = OpCode.OuterProduct; args = [ResultType; ResultId; Arg("v0", typeof<int>); Arg("v1", typeof<int>) ] }
        { opCode = OpCode.Dot; args = [ResultType; ResultId; Arg("v0", typeof<int>); Arg("v1", typeof<int>) ] }
        { opCode = OpCode.IAddCarry; args = [ResultType; ResultId] }
        { opCode = OpCode.ISubBorrow; args = [ResultType; ResultId] }
        { opCode = OpCode.IMulExtended; args = [ResultType; ResultId] }

        // Bit instructions
        { opCode = OpCode.ShiftRightLogical; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.ShiftRightArithmetic; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.ShiftLeftLogical; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.BitwiseOr; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.BitwiseXor; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.BitwiseAnd; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.Not; args = [ResultType; ResultId; Arg("v", typeof<int>) ] }
        { opCode = OpCode.BitFieldInsert; args = [ResultType; ResultId; Arg("_base", typeof<int>); Arg("insert", typeof<int>); Arg("offset", typeof<int>); Arg("count", typeof<int>) ] }
        { opCode = OpCode.BitFieldSExtract; args = [ResultType; ResultId; Arg("_base", typeof<int>); Arg("offset", typeof<int>); Arg("count", typeof<int>) ] }
        { opCode = OpCode.BitFieldUExtract; args = [ResultType; ResultId; Arg("_base", typeof<int>); Arg("offset", typeof<int>); Arg("count", typeof<int>) ] }
        { opCode = OpCode.BitReverse; args = [ResultType; ResultId; Arg("_base", typeof<int>) ] }
        { opCode = OpCode.BitCount; args = [ResultType; ResultId; Arg("_base", typeof<int>) ] }

        // Relations and Logics
        { opCode = OpCode.Any; args = [ResultType; ResultId; Arg("vec", typeof<int>) ] }
        { opCode = OpCode.All; args = [ResultType; ResultId; Arg("vec", typeof<int>) ] }
        { opCode = OpCode.IsNan; args = [ResultType; ResultId; Arg("v", typeof<int>) ] }
        { opCode = OpCode.IsInf; args = [ResultType; ResultId; Arg("v", typeof<int>) ] }
        { opCode = OpCode.IsFinite; args = [ResultType; ResultId; Arg("v", typeof<int>) ] }
        { opCode = OpCode.IsNormal; args = [ResultType; ResultId; Arg("v", typeof<int>) ] }
        { opCode = OpCode.SignBitSet; args = [ResultType; ResultId; Arg("v", typeof<int>) ] }
        { opCode = OpCode.LessOrGreater; args = [ResultType; ResultId; Arg("x", typeof<int>); Arg("y", typeof<int>) ] }
        { opCode = OpCode.Ordered; args = [ResultType; ResultId; Arg("x", typeof<int>); Arg("y", typeof<int>) ] }
        { opCode = OpCode.Unordered; args = [ResultType; ResultId; Arg("x", typeof<int>); Arg("y", typeof<int>) ] }
        { opCode = OpCode.LogicalEqual; args = [ResultType; ResultId; Arg("x", typeof<int>); Arg("y", typeof<int>) ] }
        { opCode = OpCode.LogicalNotEqual; args = [ResultType; ResultId; Arg("x", typeof<int>); Arg("y", typeof<int>) ] }
        { opCode = OpCode.LogicalOr; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.LogicalAnd; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.LogicalNot; args = [ResultType; ResultId; Arg("v", typeof<int>) ] }
        { opCode = OpCode.Select; args = [ResultType; ResultId; Arg("guard", typeof<int>); Arg("vtrue", typeof<int>); Arg("vfalse", typeof<int>) ] }
        { opCode = OpCode.IEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.INotEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.UGreaterThan; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.SGreaterThan; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.UGreaterThanEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.SGreaterThanEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.ULessThan; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.SLessThan; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.ULessThanEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.SLessThanEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FOrdEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FUnordEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FOrdNotEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FUnordNotEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FOrdLessThan; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FUnordLessThan; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FOrdGreaterThan; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FUnordGreaterThan; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FOrdLessThanEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FUnordLessThanEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FOrdGreaterThanEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }
        { opCode = OpCode.FUnordGreaterThanEqual; args = [ResultType; ResultId; Arg("l", typeof<int>); Arg("r", typeof<int>) ] }


        // Derivative instruction
        { opCode = OpCode.DPdx; args = [ResultType; ResultId; Arg("p", typeof<int>) ] }
        { opCode = OpCode.DPdy; args = [ResultType; ResultId; Arg("p", typeof<int>) ] }
        { opCode = OpCode.Fwidth; args = [ResultType; ResultId; Arg("p", typeof<int>) ] }
        { opCode = OpCode.DPdxFine; args = [ResultType; ResultId; Arg("p", typeof<int>) ] }
        { opCode = OpCode.DPdyFine; args = [ResultType; ResultId; Arg("p", typeof<int>) ] }
        { opCode = OpCode.FwidthFine; args = [ResultType; ResultId; Arg("p", typeof<int>) ] }
        { opCode = OpCode.DPdxCoarse; args = [ResultType; ResultId; Arg("p", typeof<int>) ] }
        { opCode = OpCode.DPdyCoarse; args = [ResultType; ResultId; Arg("p", typeof<int>) ] }
        { opCode = OpCode.FwidthCoarse; args = [ResultType; ResultId; Arg("p", typeof<int>) ] }

        // Control Flow
        { opCode = OpCode.Phi; args = [ResultType; ResultId; Arg("variableParentPairs", typeof<int[]>) ] }
        { opCode = OpCode.LoopMerge; args = [Arg("mergeBlock", typeof<int>); Arg("ctrl", typeof<LoopControl>) ] }
        { opCode = OpCode.SelectionMerge; args = [Arg("mergeBlock", typeof<int>); Arg("ctrl", typeof<SelectionControl>) ] }
        { opCode = OpCode.Label; args = [ResultId] }
        { opCode = OpCode.Branch; args = [Arg("target", typeof<int>)] }
        { opCode = OpCode.BranchConditional; args = [Arg("condition", typeof<int>); Arg("trueLabel", typeof<int>); Arg("falseLabel", typeof<int>); Arg("branchWeights", typeof<int[]>)] }
        { opCode = OpCode.Switch; args = [Arg("selector", typeof<int>); Arg("_default", typeof<int>); Arg("literalLabelPairs", typeof<int[]>)] }
        { opCode = OpCode.Kill; args = [] }
        { opCode = OpCode.Return; args = [] }
        { opCode = OpCode.ReturnValue; args = [Arg("value", typeof<int>)] }
        { opCode = OpCode.Unreachable; args = [] }
        { opCode = OpCode.LifetimeStart; args = [Arg("ptr", typeof<int>); Arg("size", typeof<int>)] }
        { opCode = OpCode.LifetimeStop; args = [Arg("ptr", typeof<int>); Arg("size", typeof<int>)] }

        // Atomics
        { opCode = OpCode.AtomicLoad; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>)] }
        { opCode = OpCode.AtomicStore; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("value", typeof<int>)] }
        { opCode = OpCode.AtomicExchange; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>); Arg("value", typeof<int>)] }
        { opCode = OpCode.AtomicCompareExchange; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("_equal", typeof<MemorySemantics>); Arg("_unequal", typeof<MemorySemantics>); Arg("value", typeof<int>); Arg("cmp", typeof<int>)] }
        { opCode = OpCode.AtomicCompareExchangeWeak; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("_equal", typeof<MemorySemantics>); Arg("_unequal", typeof<MemorySemantics>); Arg("value", typeof<int>); Arg("cmp", typeof<int>)] }
        { opCode = OpCode.AtomicIIncrement; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>)] }
        { opCode = OpCode.AtomicIDecrement; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>)] }
        { opCode = OpCode.AtomicIAdd; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>); Arg("value", typeof<int>)] }
        { opCode = OpCode.AtomicISub; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>); Arg("value", typeof<int>)] }
        { opCode = OpCode.AtomicSMin; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>); Arg("value", typeof<int>)] }
        { opCode = OpCode.AtomicUMin; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>); Arg("value", typeof<int>)] }
        { opCode = OpCode.AtomicSMax; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>); Arg("value", typeof<int>)] }
        { opCode = OpCode.AtomicUMax; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>); Arg("value", typeof<int>)] }
        { opCode = OpCode.AtomicAnd; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>); Arg("value", typeof<int>)] }
        { opCode = OpCode.AtomicOr; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>); Arg("value", typeof<int>)] }
        { opCode = OpCode.AtomicXor; args = [ResultType; ResultId; Arg("ptr", typeof<int>); Arg("scope", typeof<Scope>); Arg("sem", typeof<MemorySemantics>); Arg("value", typeof<int>)] }
        
        // Primitive instructions
        { opCode = OpCode.EmitVertex; args = [] }
        { opCode = OpCode.EndPrimitive; args = [] }
        { opCode = OpCode.EmitStreamVertex; args = [Arg("stream", typeof<int>)] }
        { opCode = OpCode.EndStreamPrimitive; args = [Arg("stream", typeof<int>)] }

        // Barrier instructions
        { opCode = OpCode.ControlBarrier; args = [Arg("exec", typeof<Scope>); Arg("mem", typeof<Scope>); Arg("sem", typeof<MemorySemantics>)] }
        { opCode = OpCode.MemoryBarrier; args = [Arg("mem", typeof<Scope>); Arg("sem", typeof<MemorySemantics>)] }

        // !!!currently omitted!!!!

        // Group instructions


        // Device-Side enqueue instructions


        // Pipe instructions

    ]



let header() =
    printfn "namespace GLSLang.SpirV"
    printfn ""
    printfn "open System.IO"
    printfn ""

let rec typeName (t : Type) =
    if t = typeof<int> then "uint32"
    elif t = typeof<string> then "string"
    elif t = typeof<bool> then "bool"
    elif t.IsArray then sprintf "%s[]" (typeName (t.GetElementType()))
    else t.Name

let definition() =
    printfn "type Instruction = "
    for p in prototypes do
        if List.isEmpty p.args then
            printfn "    | %A" p.opCode
        else
            let args = 
                p.args 
                    |> List.map (fun a ->
                        match a with
                            | ResultType -> "resType : uint32"
                            | ResultId -> "resId : uint32"
                            | Arg(n, t) -> sprintf "%s : %s" n (typeName t)
                        )
                    |> String.concat " * "
            printfn "    | %A of %s" p.opCode args

    printfn ""
    printfn ""

let readerString (a : Argument) (offset : byref<int>) =
    match a with
        | ResultId | ResultType -> 
            let res = sprintf "args.UInt32 %d" (offset / 4)
            offset <- offset + 4
            res
        | Arg(name, t) ->
            if t = typeof<string> then
                sprintf "args.String %d" offset
            elif t = typeof<int> then
                let res = sprintf "args.UInt32 %d" (offset / 4)
                offset <- offset + 4
                res
            elif t.IsEnum then
                let res = sprintf "args.UInt32 %d |> unbox<%s>" (offset / 4) t.Name
                offset <- offset + 4
                res
            elif t = typeof<bool> then
                let res = sprintf "args.UInt32 %d = 1u" (offset / 4)
                offset <- offset + 4
                res
            elif t.IsArray && t.GetElementType() = typeof<int> then
                sprintf "args.UInt32Array %d" (offset / 4)
            else
                failwithf "cannot read: %A" t

let operationHeader() =
    printfn "module SpirVUtilities = "

let readers() =
    printfn "    let private ofRawInstruction (i : RawInstruction) = "
    printfn "        let args = i.operands"
    printfn "        match i.opCode with"
    for p in prototypes do
        if List.isEmpty p.args then
            printfn "            | OpCode.%A -> %A" p.opCode p.opCode
        else
            let readers = 
                [
                    let offset = ref 0
                    for a in p.args do
                        let str = readerString a &offset.contents 
                        yield str

                ] |> String.concat ", "

            printfn "            | OpCode.%A -> %A(%s)" p.opCode p.opCode readers
    printfn "            | code -> failwithf \"unknown OpCode: %%A\" code"
    printfn ""
    printfn "    let readStream (i : Stream) = "
    printfn "        let m = RawReader.read i"
    printfn "        m.instructions |> List.map ofRawInstruction"
    printfn ""
    printfn ""

let writers() =
    printfn "    let private toRawInstruction (i : Instruction) = "
    printfn "        match i with"
    for p in prototypes do
        if List.isEmpty p.args then
            printfn "            | %A -> { opCode = OpCode.%A; operands = RawOperands() }" p.opCode p.opCode
        else
            let args = 
                p.args 
                    |> List.map (fun a ->
                        match a with
                            | ResultType -> "resType"
                            | ResultId -> "resId"
                            | Arg(name, _) -> name
                       )
                    |> String.concat ", "

            printfn "            | %A(%s) -> { opCode = OpCode.%A; operands = RawOperands(%s) }" p.opCode args p.opCode args

    printfn ""
    printfn "    let writeStream (o : Stream) (instructions : list<Instruction>) = "
    printfn "        let raw = instructions |> List.map toRawInstruction"
    printfn "        let maxId = instructions |> List.choose tryGetId |> List.max"
    printfn "        RawWriter.write o raw maxId"
    printfn ""
    printfn ""

let reflector() =
    printfn "    let tryGetId (i : Instruction) = "
    printfn "        match i with"
    for p in prototypes do
        if not (List.isEmpty p.args) then
            let found = ref false
            let args = 
                p.args 
                    |> List.map (fun a ->
                        match a with
                            | ResultType -> "_"
                            | ResultId -> 
                                found := true
                                "resId"
                            | Arg(name, _) -> "_"
                       )
                    |> String.concat ", "

            if !found then
                printfn "            | %A(%s) -> Some resId" p.opCode args

    printfn "            | _ -> None"
    printfn ""


    printfn "    let tryGetResultTypeId (i : Instruction) = "
    printfn "        match i with"
    for p in prototypes do
        if not (List.isEmpty p.args) then
            let found = ref false
            let args = 
                p.args 
                    |> List.map (fun a ->
                        match a with
                            | ResultId -> "_"
                            | ResultType -> 
                                found := true
                                "resType"
                            | Arg(name, _) -> "_"
                       )
                    |> String.concat ", "

            if !found then
                printfn "            | %A(%s) -> Some resType" p.opCode args

    printfn "            | _ -> None"
    printfn ""
    



let generate() =
    header()
    definition()

    operationHeader()
    readers()
    reflector()
    writers()

    let content = sb.ToString()
    File.WriteAllText("SpirVInstructions.fs", content)