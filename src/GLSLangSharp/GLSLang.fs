namespace GLSLang

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
open Aardvark.Base

#nowarn "9"

type ShaderStage =
    | Vertex = 0
    | TessControl = 1
    | TessEvaluation = 2
    | Geometry = 3
    | Fragment = 4
    | Compute = 5
    | RayGen = 6
    | RayIntersect = 7
    | RayAnyHit = 8
    | RayClosestHit = 9
    | RayMiss = 10
    | Callable = 11
    | Task = 12
    | Mesh = 13

[<AutoOpen>]
module private ``GLSLang internal functions`` =
    let mutable private initialized = false

    let init() =
        if not initialized then
            initialized <- true
            let success = Raw.GLSLang.ShInitializeProcess()
            if not success then
                failwith "could not initialize GLSLang process"


[<RequireQualifiedAccess>]
type Optimization =
    /// Apply the conditional constant propagation transform.  This will
    /// propagate constant values throughout the program, and simplify
    /// expressions and conditional jumps with known predicate
    /// values.  Performed on entry point call tree functions and
    /// exported functions.
    | Ccp
    /// Cleanup the control flow graph. This will remove any unnecessary
    /// code from the CFG like unreachable code. Performed on entry
    /// point call tree functions and exported functions.
    | CfgCleanup
    /// Combines chained access chains to produce a single instruction
    /// where possible.
    | CombineAccessChains
    /// Remap result ids to a compact range starting from %1 and without
    /// any gaps.
    | CompactIds
    /// Convert constant index access chain loads/stores into
    /// equivalent load/stores with inserts and extracts. Performed
    /// on function scope variables referenced only with load, store,
    /// and constant index access chains in entry point call tree
    /// functions.
    | ConvertLocalAccessChains
    /// Does propagation of memory references when an array is a copy of
    /// another.  It will only propagate an array if the source is never
    /// written to, and the only store to the target is the copy.
    | CopyPropagateArrays
    /// Perform load/load elimination for duplicate uniform values.
    /// Converts any constant index access chain uniform loads into
    /// its equivalent load and extract. Some loads will be moved
    /// to facilitate sharing. Performed only on entry point
    /// call tree functions.
    | EliminateCommonUniform
    /// Convert conditional branches with constant condition to the
    /// indicated unconditional brranch. Delete all resulting dead
    /// code. Performed only on entry point call tree functions.
    | EliminateDeadBranches
    /// Delete instructions which do not contribute to a function's
    /// output. Performed only on entry point call tree functions.
    | EliminateDeadCodeAggressive
    /// Eliminate dead constants.
    | EliminateDeadConst
    /// Deletes functions that cannot be reached from entry points or
    /// exported functions.
    | EliminateDeadFunctions
    /// Deletes unreferenced inserts into composites, most notably
    /// unused stores to vector components, that are not removed by
    /// aggressive dead code elimination.
    | EliminateDeadInsert
    /// Deletes module scope variables that are not referenced.
    | EliminateDeadVariables
    /// DEPRECATED.  This pass has been replaced by the simplification
    /// pass, and that pass will be run instead.
    /// See --simplify-instructions.
    | EliminateInsertExtract
    /// Replace stores and loads of function scope variables that are
    /// stored multiple times. Performed on variables referenceed only
    /// with loads and stores. Performed only on entry point call tree
    /// functions.
    | EliminateLocalMultiStore
    /// Perform single-block store/load and load/load elimination.
    /// Performed only on function scope variables in entry point
    /// call tree functions.
    | EliminateLocalSingleBlock
    /// Replace stores and loads of function scope variables that are
    /// only stored once. Performed on variables referenceed only with
    /// loads and stores. Performed only on entry point call tree
    /// functions.
    | EliminateLocalSingleStore
    /// Replace decoration groups with repeated OpDecorate and
    /// OpMemberDecorate instructions.
    | FlattenDecorations
    /// Fold the spec constants defined by OpSpecConstantOp or
    /// OpSpecConstantComposite instructions to front-end constants
    /// when possible.
    | FoldSpecConstOpComposite
    /// Freeze the values of specialization constants to their default
    /// values.
    | FreezeSpecConst
    /// Convert if-then-else like assignments into OpSelect.
    | IfConversion
    /// Exhaustively inline all function calls in entry point call tree
    /// functions. Currently does not inline calls to functions with
    /// early return in a loop.
    | InlineEntryPointsExhaustive
    /// Runs a series of optimizations that attempts to take SPIR-V
    /// generated by an HLSL front-end and generates legal Vulkan SPIR-V.
    /// The optimizations are:

    ///  eliminate-dead-branches
    ///  merge-return
    ///  inline-entry-points-exhaustive
    ///  eliminate-dead-functions
    ///  private-to-local
    ///  eliminate-local-single-block
    ///  eliminate-local-single-store
    ///  eliminate-dead-code-aggressive
    ///  scalar-replacement=0
    ///  eliminate-local-single-block
    ///  eliminate-local-single-store
    ///  eliminate-dead-code-aggressive
    ///  eliminate-local-multi-store
    ///  eliminate-dead-code-aggressive
    ///  ccp
    ///  eliminate-dead-branches
    ///  simplify-instructions
    ///  eliminate-dead-code-aggressive
    ///  copy-propagate-arrays
    ///  vector-dce
    ///  eliminate-dead-inserts
    ///  reduce-load-size
    ///  eliminate-dead-code-aggressive

    /// Note this does not guarantee legal code. This option passes the
    /// option --relax-logical-pointer to the validator.
    | LegalizeHlsl
    /// Looks for instructions in the same basic block that compute the
    /// same value, and deletes the redundant ones.
    | LocalRedundancyElimination
    /// Splits any top level loops in which the register pressure has exceeded
    /// a given threshold. The threshold must follow the use of this flag and
    /// must be a positive integer value.
    | LoopFission of int
    /// Identifies adjacent loops with the same lower and upper bound.
    /// If this is legal, then merge the loops into a single loop.
    /// Includes heuristics to ensure it does not increase number of
    /// registers too much, while reducing the number of loads from
    /// memory. Takes an additional positive integer argument to set
    /// the maximum number of registers.
    | LoopFusion
    /// Fully unrolls loops marked with the Unroll flag
    | LoopUnroll
    /// Partially unrolls loops marked with the Unroll flag. Takes an
    /// additional non-0 integer argument to set the unroll factor, or
    /// how many times a loop body should be duplicated
    | LoopUnrollPartial
    /// Execute few first (respectively last) iterations before
    /// (respectively after) the loop if it can elide some branches.
    | LoopPeeling
    /// Takes a non-0 integer argument to set the loop peeling code size
    /// growth threshold. The threshold prevents the loop peeling
    /// from happening if the code size increase created by
    /// the optimization is above the threshold.
    | LoopPeelingThreshold
    /// Join two blocks into a single block if the second has the
    /// first as its only predecessor. Performed only on entry point
    /// call tree functions.
    | MergeBlocks
    /// Changes functions that have multiple return statements so they
    /// have a single return statement.

    /// For structured control flow it is assumed that the only
    /// unreachable blocks in the function are trivial merge and continue
    /// blocks.

    /// A trivial merge block contains the label and an OpUnreachable
    /// instructions, nothing else.  A trivial continue block contain a
    /// label and an OpBranch to the header, nothing else.

    /// These conditions are guaranteed to be met after running
    /// dead-branch elimination.
    | MergeReturn

    /// Hoists loop-invariant conditionals out of loops by duplicating
    /// the loop on each branch of the conditional and adjusting each
    /// copy of the loop.
    | LoopUnswitch
    /// Print SPIR-V assembly to standard error output before each pass
    /// and after the last pass.
    | PrintAll
    /// Change the scope of private variables that are used in a single
    /// function to that function.
    | PrivateToLocal
    /// Replaces loads of composite objects where not every component is
    /// used by loads of just the elements that are used.
    | ReduceLoadSize
    /// Looks for instructions in the same function that compute the
    /// same value, and deletes the redundant ones.
    | RedundancyElimination
    /// Allow store from one struct type to a different type with
    /// compatible layout and members. This option is forwarded to the
    /// validator.
    | RelaxStructStore
    /// Removes duplicate types, decorations, capabilities and extension
    /// instructions.
    | RemoveDuplicates
    /// Replaces instructions whose opcode is valid for shader modules,
    /// but not for the current shader stage.  To have an effect, all
    /// entry points must have the same execution model.
    | ReplaceInvalidOpcode
    /// Replace loads and stores to function local variables with
    /// operations on SSA IDs.
    | SsaRewrite
    /// Replace aggregate function scope variables that are only accessed
    /// via their elements with new function variables representing each
    /// element.  <n> is a limit on the size of the aggragates that will
    /// be replaced.  0 means there is no limit.  The default value is
    /// 100.
    | ScalarReplacement of int

    /// Set the default values of the specialization constants with
    /// <spec id>:<default value> pairs specified in a double-quoted
    /// string. <spec id>:<default value> pairs must be separated by
    /// blank spaces, and in each pair, spec id and default value must
    /// be separated with colon ':' without any blank spaces in between.
    /// e.g.: --set-spec-const-default-value "1:100 2:400"
    | SetSpecConstDefaultValue of Map<int, string> //"<spec id>:<default value> ..."
    /// Will simplify all instructions in the function as much as
    /// possible.
    | SimplifyInstructions
    /// Will not validate the SPIR-V before optimizing.  If the SPIR-V
    /// is invalid, the optimizer may fail or generate incorrect code.
    /// This options should be used rarely, and with caution.
    | SkipValidation
    /// Replaces instructions with equivalent and less expensive ones.
    | StrengthReduction
    /// Remove all debug instructions.
    | StripDebug
    /// Remove all reflection information.  For now, this covers
    /// reflection information defined by SPV_GOOGLE_hlsl_functionality1.
    | StripReflect
    /// Print the resource utilization of each pass (e.g., CPU time,
    /// RSS) to standard error output. Currently it supports only Unix
    /// systems. This option is the same as -ftime-report in GCC. It
    /// prints CPU/WALL/USR/SYS time (and RSS if possible), but note that
    /// USR/SYS time are returned by getrusage() and can have a small
    /// error.
    | TimeReport
    /// This pass looks for components of vectors that are unused, and
    /// removes them from the vector.  Note this would still leave around
    /// lots of dead code that a pass of ADCE will be able to remove.
    | VectorDce
    /// Rewrites instructions for which there are known driver bugs to
    /// avoid triggering those bugs.
    /// Current workarounds: Avoid OpUnreachable in loops.
    | Workaround1209
    /// Remove the duplicated constants.
    | UnifyConst

module Optimization =

    let toString (o : Optimization) =
        match o with
            | Optimization.Ccp                                         -> "--ccp"
            | Optimization.CfgCleanup                                  -> "--cfg-cleanup"
            | Optimization.CombineAccessChains                         -> "--combine-access-chains"
            | Optimization.CompactIds                                  -> "--compact-ids"
            | Optimization.ConvertLocalAccessChains                    -> "--convert-local-access-chains"
            | Optimization.CopyPropagateArrays                         -> "--copy-propagate-arrays"
            | Optimization.EliminateCommonUniform                      -> "--eliminate-common-uniform"
            | Optimization.EliminateDeadBranches                       -> "--eliminate-dead-branches"
            | Optimization.EliminateDeadCodeAggressive                 -> "--eliminate-dead-code-aggressive"
            | Optimization.EliminateDeadConst                          -> "--eliminate-dead-const"
            | Optimization.EliminateDeadFunctions                      -> "--eliminate-dead-functions"
            | Optimization.EliminateDeadInsert                         -> "--eliminate-dead-insert"
            | Optimization.EliminateDeadVariables                      -> "--eliminate-dead-variables"
            | Optimization.EliminateInsertExtract                      -> "--eliminate-insert-extract"
            | Optimization.EliminateLocalMultiStore                    -> "--eliminate-local-multi-store"
            | Optimization.EliminateLocalSingleBlock                   -> "--eliminate-local-single-block"
            | Optimization.EliminateLocalSingleStore                   -> "--eliminate-local-single-store"
            | Optimization.FlattenDecorations                          -> "--flatten-decorations"
            | Optimization.FoldSpecConstOpComposite                    -> "--fold-spec-const-op-composite"
            | Optimization.FreezeSpecConst                             -> "--freeze-spec-const"
            | Optimization.IfConversion                                -> "--if-conversion"
            | Optimization.InlineEntryPointsExhaustive                 -> "--inline-entry-points-exhaustive"
            | Optimization.LegalizeHlsl                                -> "--legalize-hlsl"
            | Optimization.LocalRedundancyElimination                  -> "--local-redundancy-elimination"
            | Optimization.LoopFission v                               -> sprintf "--loop-fission=%d" v
            | Optimization.LoopFusion                                  -> "--loop-fusion"
            | Optimization.LoopUnroll                                  -> "--loop-unroll"
            | Optimization.LoopUnrollPartial                           -> "--loop-unroll-partial"
            | Optimization.LoopPeeling                                 -> "--loop-peeling"
            | Optimization.LoopPeelingThreshold                        -> "--loop-peeling-threshold"
            | Optimization.MergeBlocks                                 -> "--merge-blocks"
            | Optimization.MergeReturn                                 -> "--merge-return"
            | Optimization.LoopUnswitch                                -> "--loop-unswitch"
            | Optimization.PrintAll                                    -> "--print-all"
            | Optimization.PrivateToLocal                              -> "--private-to-local"
            | Optimization.ReduceLoadSize                              -> "--reduce-load-size"
            | Optimization.RedundancyElimination                       -> "--redundancy-elimination"
            | Optimization.RelaxStructStore                            -> "--relax-struct-store"
            | Optimization.RemoveDuplicates                            -> "--remove-duplicates"
            | Optimization.ReplaceInvalidOpcode                        -> "--replace-invalid-opcode"
            | Optimization.SsaRewrite                                  -> "--ssa-rewrite"
            | Optimization.ScalarReplacement n                         -> sprintf "--scalar-replacement=%d" n
            | Optimization.SimplifyInstructions                        -> "--simplify-instructions"
            | Optimization.SkipValidation                              -> "--skip-validation"
            | Optimization.StrengthReduction                           -> "--strength-reduction"
            | Optimization.StripDebug                                  -> "--strip-debug"
            | Optimization.StripReflect                                -> "--strip-reflect"
            | Optimization.TimeReport                                  -> "--time-report"
            | Optimization.VectorDce                                   -> "--vector-dce"
            | Optimization.Workaround1209                              -> "--workaround-1209"
            | Optimization.UnifyConst                                  -> "--unify-const"
            | Optimization.SetSpecConstDefaultValue map                -> 
                let values = map |> Map.toList |> List.map (fun (id,value) -> sprintf "%d:%s" id value) |> String.concat " "
                sprintf "--set-spec-const-default-value=%s" values

    let Default =
        [
            Optimization.SkipValidation

            Optimization.MergeReturn
            Optimization.InlineEntryPointsExhaustive
            Optimization.EliminateDeadCodeAggressive
            Optimization.PrivateToLocal
            Optimization.EliminateLocalSingleBlock
            Optimization.EliminateLocalSingleStore
            Optimization.EliminateDeadCodeAggressive
            Optimization.ScalarReplacement 100
            Optimization.ConvertLocalAccessChains
            Optimization.EliminateLocalSingleBlock
            Optimization.EliminateLocalSingleStore
            Optimization.EliminateDeadCodeAggressive
            Optimization.EliminateLocalMultiStore
            Optimization.EliminateDeadCodeAggressive
            Optimization.Ccp
            Optimization.EliminateDeadCodeAggressive
            Optimization.RedundancyElimination
            Optimization.CombineAccessChains
            Optimization.SimplifyInstructions
            Optimization.VectorDce
            Optimization.EliminateDeadBranches
            Optimization.SimplifyInstructions
            Optimization.IfConversion
            Optimization.CopyPropagateArrays
            Optimization.ReduceLoadSize
            Optimization.EliminateDeadCodeAggressive
            Optimization.MergeBlocks
            Optimization.RedundancyElimination
            Optimization.EliminateDeadBranches
            Optimization.MergeBlocks
            Optimization.SimplifyInstructions
            
        ]


module GLSLang =
    
    let tryCompile (stage : ShaderStage) (entryName : string) (defines : list<string>) (source : string) =
        init()

        let lang = stage |> int |> unbox<Raw.ShLanguage>
        let defines = List.toArray defines

        let mutable ptr = 0n
        let mutable size = 0un

        let mutable logLength = 0
        let mutable log = 0n

        try
            match Raw.GLSLang.ShCompileShader(lang, entryName, source, defines.Length, defines, &size, &ptr, &logLength, &log) with
                | 0 ->
                    let arr : byte[] = Array.zeroCreate (int size)
                    Marshal.Copy(ptr, arr, 0, int size)
                    let str = Marshal.PtrToStringAnsi(log, int logLength)
                    (Some arr, str)
                | _ ->
                    let str = Marshal.PtrToStringAnsi(log, int logLength)
                    (None, str)
        finally
            if ptr <> 0n then Raw.GLSLang.ShFree ptr
            if log <> 0n then Raw.GLSLang.ShFree log
            
    let optimize (passes : list<Optimization>) (binary : byte[]) =
        let passNames = passes |> Seq.map Optimization.toString |> Seq.toArray

        let gc = GCHandle.Alloc(binary, GCHandleType.Pinned)
        let mutable output = 0n
        let mutable outputLength = 0un
        try Raw.GLSLang.ShOptimize(gc.AddrOfPinnedObject(), unativeint binary.LongLength, &output, &outputLength, passNames, passNames.Length)
        finally gc.Free()

        let arr : byte[] = Array.zeroCreate (int outputLength)
        Marshal.Copy(output, arr, 0, arr.Length)
        Raw.GLSLang.ShFree(output)

        arr

    let optimizeDefault (binary : byte[]) =

        let gc = GCHandle.Alloc(binary, GCHandleType.Pinned)
        let mutable output = 0n
        let mutable outputLength = 0un
        try Raw.GLSLang.ShOptimize(gc.AddrOfPinnedObject(), unativeint binary.LongLength, &output, &outputLength, null, 0)
        finally gc.Free()

        let arr : byte[] = Array.zeroCreate (int outputLength)
        Marshal.Copy(output, arr, 0, arr.Length)
        Raw.GLSLang.ShFree(output)

        arr


