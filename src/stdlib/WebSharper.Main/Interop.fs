// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2016 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

module private WebSharper.InteropProxy

open WebSharper.JavaScript

type PA = System.ParamArrayAttribute

[<Proxy(typeof<Function>)>]
type FunctionProxy =
    [<Inline "Function.constructor.apply(null, $paramsAndBody)">]
    new ([<System.ParamArray>] paramsAndBody: string[]) = {}
    
    member this.Length with [<Inline "$this.length">] get() = X<int>
    
    [<Inline "$this.apply($thisArg)">]
    member this.ApplyUnsafe(thisArg: obj) = X<obj>

    [<Inline "$this.apply($thisArg, $argsArray)">]
    member this.ApplyUnsafe(thisArg: obj, argsArray: obj[]) = X<obj>

    [<Inline "$this.apply($thisArg, $args)">]
    member this.CallUnsafe(thisArg: obj, [<PA>] args: obj[]) = X<obj>
  
    [<Inline "Function.prototype.bind.apply($thisArg, $args)">]
    member this.BindUnsafe(thisArg: obj, [<PA>] args: obj[]) = Unchecked.defaultof<Function>

    [<Inline "$func">]
    static member Of<'T, 'U>(func: 'T -> 'U) = X<Function>

[<Proxy(typeof<FuncWithArgs<_,_>>)>]
type FuncWithArgsProxy<'TArgs, 'TResult> =
    [<Macro(typeof<WebSharper.Macro.FuncWithArgs>)>]
    new (func: 'TArgs -> 'TResult) = {}
    
    member this.Length with [<Inline "$this.length">] get() = X<int>
       
    [<Inline "$this.apply(null, $args)">]
    member this.Call (args: 'Args) = X<'TResult>

[<Proxy(typeof<FuncWithThis<_,_>>)>]
type FuncWithThisProxy<'TThis, 'TFunc> =
//    [<Macro(typeof<WebSharper.Macro.FuncWithThis>)>]
    [<Inline "$wsruntime.CreateFuncWithThis($func)">]
    new (func: 'TThis -> 'TFunc) = {}

    member this.Length with [<Inline "$this.length">] get() = 0

    [<Inline "$wsruntime.Bind($this, $thisArg)">]
    member this.Bind (thisArg: 'TThis) = X<'TFunc>

[<Proxy(typeof<FuncWithOnlyThis<_,_>>)>]
type FuncWithOnlyThisProxy<'TThis, 'TResult> =
    [<Inline "$wsruntime.CreateFuncWithOnlyThis($func)">]
    new (func: 'TThis -> 'TResult) = {}

    member this.Length with [<Inline "$this.length">] get() = 0

    [<Inline "$this.call($thisArg)">]
    member this.Call (thisArg: 'TThis) = X<unit>

[<Proxy(typeof<FuncWithRest<_,_>>)>]
type FuncWithRestProxy<'TRest, 'TResult> =
    [<Inline "$wsruntime.CreateFuncWithArgs($func)">]
    new (func: 'TRest[] -> 'TResult) = {}

    [<Inline "$this.apply(null, $rest)">]
    member this.Call ([<PA>] rest: 'TRest[]) = X<'TResult>

[<Proxy(typeof<FuncWithRest<_,_,_>>)>]
type FuncWithRestProxy<'TArg, 'TRest, 'TResult> =
    [<Inline "$wsruntime.CreateFuncWithRest(1, $func)">]
    new (func: 'TArg * 'TRest[] -> 'TResult) = {}

    [<Inline "$this.apply(null, [$arg].concat($rest))">]
    member this.Call (arg: 'TArg, [<PA>] rest: 'TRest[]) = X<'TResult>

[<Proxy(typeof<FuncWithRest<_,_,_,_>>)>]
type FuncWithRestProxy<'TArg1, 'TArg2, 'TRest, 'TResult> =
    [<Inline "$wsruntime.CreateFuncWithRest(2, $func)">]
    new (func: 'TArg1 * 'TArg2 * 'TRest[] -> 'TResult) = {}

    [<Inline "$this.apply(null, [$arg1, $arg2].concat($rest))">]
    member this.Call (arg1: 'TArg1, arg2: 'TArg2, [<PA>] rest: 'TRest[]) = Unchecked.defaultof<'TResult>

[<Proxy(typeof<FuncWithRest<_,_,_,_,_>>)>]
type FuncWithRestProxy<'TArg1, 'TArg2, 'TArg3, 'TRest, 'TResult> =
    [<Inline "$wsruntime.CreateFuncWithRest(3, $func)">]
    new (func: 'TArg1 * 'TArg2 * 'TArg3 * 'TRest[] -> 'TResult) = {}

    [<Inline "$this.apply(null, [$arg1, $arg2, $arg3].concat($rest))">]
    member this.Call (arg1: 'TArg1, arg2: 'TArg2, arg3: 'TArg3, [<PA>] rest: 'TRest[]) = Unchecked.defaultof<'TResult>

[<Proxy(typeof<FuncWithRest<_,_,_,_,_,_>>)>]
type FuncWithRestProxy<'TArg1, 'TArg2, 'TArg3, 'TArg4, 'TRest, 'TResult> =
    [<Inline "$wsruntime.CreateFuncWithRest(4, $func)">]
    new (func: 'TArg1 * 'TArg2 * 'TArg3 * 'TArg4 * 'TRest[] -> 'TResult) = {}

    [<Inline "$this.apply(null, [$arg1, $arg2, $arg3, $arg4].concat($rest))">]
    member this.Call (arg1: 'TArg1, arg2: 'TArg2, arg3: 'TArg3, arg4: 'TArg4, [<PA>] rest: 'TRest[]) = Unchecked.defaultof<'TResult>

[<Proxy(typeof<FuncWithRest<_,_,_,_,_,_,_>>)>]
type FuncWithRestProxy<'TArg1, 'TArg2, 'TArg3, 'TArg4, 'TArg5, 'TRest, 'TResult> =
    [<Inline "$wsruntime.CreateFuncWithRest(5, $func)">]
    new (func: 'TArg1 * 'TArg2 * 'TArg3 * 'TArg4 * 'TArg5 * 'TRest[] -> 'TResult) = {}

    [<Inline "$this.apply(null, [$arg1, $arg2, $arg3, $arg4, $arg5].concat($rest))">]
    member this.Call (arg1: 'TArg1, arg2: 'TArg2, arg3: 'TArg3, arg4: 'TArg4, arg5: 'TArg5, [<PA>] rest: 'TRest[]) = Unchecked.defaultof<'TResult>

[<Proxy(typeof<FuncWithRest<_,_,_,_,_,_,_,_>>)>]
type FuncWithRestProxy<'TArg1, 'TArg2, 'TArg3, 'TArg4, 'TArg5, 'TArg6, 'TRest, 'TResult> =
    [<Inline "$wsruntime.CreateFuncWithRest(6, $func)">]
    new (func: 'TArg1 * 'TArg2 * 'TArg3 * 'TArg4 * 'TArg5 * 'TArg6 * 'TRest[] -> 'TResult) = {}

    [<Inline "$this.apply(null, [$arg1, $arg2, $arg3, $arg4, $arg5, $arg6].concat($rest))">]
    member this.Call (arg1: 'TArg1, arg2: 'TArg2, arg3: 'TArg3, arg4: 'TArg4, arg5: 'TArg5, arg6: 'TArg6, [<PA>] rest: 'TRest[]) = Unchecked.defaultof<'TResult>

[<Proxy(typeof<FuncWithArgsRest<_,_,_>>)>]
type FuncWithArgsRestProxy<'TArgs, 'TRest, 'TResult> =
//    [<Macro(typeof<WebSharper.Macro.FuncWithArgsRest>)>]
    [<Inline "$wsruntime.CreateFuncWithArgsRest($func)">]
    new (func: 'TArgs * 'TRest[] -> 'TResult) = {}

    [<Inline "$this.apply(null, $args.concat($rest))">]
    member this.Call (args: 'TArgs, [<PA>] rest: 'TRest[]) = Unchecked.defaultof<'TResult>

[<Proxy(typeof<Optional<_>>)>]
type OptionalProxy<'T> =
    | Undefined
    | Defined of 'T

    [<Inline>]
    member this.Value = As<'T> this

// {{ generated by genInterop.fsx, do not modify
[<Proxy (typeof<ThisAction<_>>)>]
type ThisActionProxy<'TThis> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Action<'TThis>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Action>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis) = X<unit>
[<Proxy (typeof<ThisAction<_,_>>)>]
type ThisActionProxy<'TThis, 'T> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Action<'TThis, 'T>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Action<'T>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg: 'T) = X<unit>
[<Proxy (typeof<ThisAction<_,_,_>>)>]
type ThisActionProxy<'TThis, 'T1, 'T2> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Action<'TThis, 'T1, 'T2>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Action<'T1, 'T2>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2) = X<unit>
[<Proxy (typeof<ThisAction<_,_,_,_>>)>]
type ThisActionProxy<'TThis, 'T1, 'T2, 'T3> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Action<'TThis, 'T1, 'T2, 'T3>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Action<'T1, 'T2, 'T3>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3) = X<unit>
[<Proxy (typeof<ThisAction<_,_,_,_,_>>)>]
type ThisActionProxy<'TThis, 'T1, 'T2, 'T3, 'T4> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Action<'TThis, 'T1, 'T2, 'T3, 'T4>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Action<'T1, 'T2, 'T3, 'T4>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4) = X<unit>
[<Proxy (typeof<ThisAction<_,_,_,_,_,_>>)>]
type ThisActionProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Action<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Action<'T1, 'T2, 'T3, 'T4, 'T5>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5) = X<unit>
[<Proxy (typeof<ThisAction<_,_,_,_,_,_,_>>)>]
type ThisActionProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'T6> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Action<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'T6>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Action<'T1, 'T2, 'T3, 'T4, 'T5, 'T6>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5, arg6: 'T6) = X<unit>
[<Proxy (typeof<ThisFunc<_,_>>)>]
type ThisFuncProxy<'TThis, 'TResult> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Func<'TThis, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Func<'TResult>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis) = X<'TResult>
[<Proxy (typeof<ThisFunc<_,_,_>>)>]
type ThisFuncProxy<'TThis, 'T, 'TResult> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Func<'TThis, 'T, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Func<'T, 'TResult>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg: 'T) = X<'TResult>
[<Proxy (typeof<ThisFunc<_,_,_,_>>)>]
type ThisFuncProxy<'TThis, 'T1, 'T2, 'TResult> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Func<'TThis, 'T1, 'T2, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Func<'T1, 'T2, 'TResult>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2) = X<'TResult>
[<Proxy (typeof<ThisFunc<_,_,_,_,_>>)>]
type ThisFuncProxy<'TThis, 'T1, 'T2, 'T3, 'TResult> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Func<'TThis, 'T1, 'T2, 'T3, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Func<'T1, 'T2, 'T3, 'TResult>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3) = X<'TResult>
[<Proxy (typeof<ThisFunc<_,_,_,_,_,_>>)>]
type ThisFuncProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'TResult> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Func<'TThis, 'T1, 'T2, 'T3, 'T4, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Func<'T1, 'T2, 'T3, 'T4, 'TResult>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4) = X<'TResult>
[<Proxy (typeof<ThisFunc<_,_,_,_,_,_,_>>)>]
type ThisFuncProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'TResult> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Func<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Func<'T1, 'T2, 'T3, 'T4, 'T5, 'TResult>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5) = X<'TResult>
[<Proxy (typeof<ThisFunc<_,_,_,_,_,_,_,_>>)>]
type ThisFuncProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TResult> =
    [<Inline "$wsruntime.ThisFunc($del)">]
    new (del: System.Func<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<System.Func<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TResult>>
    [<Macro(typeof<Macro.JSThisCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5, arg6: 'T6) = X<'TResult>
[<Proxy (typeof<ParamsAction<_>>)>]
type ParamsActionProxy<'TParams> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Action<'TParams>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call([<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ParamsAction<_,_>>)>]
type ParamsActionProxy<'T, 'TParams> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Action<'T, 'TParams>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg: 'T, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ParamsAction<_,_,_>>)>]
type ParamsActionProxy<'T1, 'T2, 'TParams> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Action<'T1, 'T2, 'TParams>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg1: 'T1, arg2: 'T2, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ParamsAction<_,_,_,_>>)>]
type ParamsActionProxy<'T1, 'T2, 'T3, 'TParams> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Action<'T1, 'T2, 'T3, 'TParams>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg1: 'T1, arg2: 'T2, arg3: 'T3, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ParamsAction<_,_,_,_,_>>)>]
type ParamsActionProxy<'T1, 'T2, 'T3, 'T4, 'TParams> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Action<'T1, 'T2, 'T3, 'T4, 'TParams>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ParamsAction<_,_,_,_,_,_>>)>]
type ParamsActionProxy<'T1, 'T2, 'T3, 'T4, 'T5, 'TParams> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Action<'T1, 'T2, 'T3, 'T4, 'T5, 'TParams>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ParamsAction<_,_,_,_,_,_,_>>)>]
type ParamsActionProxy<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TParams> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Action<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TParams>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5, arg6: 'T6, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ParamsFunc<_,_>>)>]
type ParamsFuncProxy<'TParams, 'TResult> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Func<'TParams, 'TResult>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call([<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ParamsFunc<_,_,_>>)>]
type ParamsFuncProxy<'T, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Func<'T, 'TParams, 'TResult>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg: 'T, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ParamsFunc<_,_,_,_>>)>]
type ParamsFuncProxy<'T1, 'T2, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Func<'T1, 'T2, 'TParams, 'TResult>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg1: 'T1, arg2: 'T2, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ParamsFunc<_,_,_,_,_>>)>]
type ParamsFuncProxy<'T1, 'T2, 'T3, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Func<'T1, 'T2, 'T3, 'TParams, 'TResult>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg1: 'T1, arg2: 'T2, arg3: 'T3, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ParamsFunc<_,_,_,_,_,_>>)>]
type ParamsFuncProxy<'T1, 'T2, 'T3, 'T4, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Func<'T1, 'T2, 'T3, 'T4, 'TParams, 'TResult>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ParamsFunc<_,_,_,_,_,_,_>>)>]
type ParamsFuncProxy<'T1, 'T2, 'T3, 'T4, 'T5, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Func<'T1, 'T2, 'T3, 'T4, 'T5, 'TParams, 'TResult>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ParamsFunc<_,_,_,_,_,_,_,_>>)>]
type ParamsFuncProxy<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ParamsFunc($del)">]
    new (del: System.Func<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TParams, 'TResult>) = { }
    [<Macro(typeof<Macro.JSParamsCall>)>]
    member this.Call(arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5, arg6: 'T6, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ThisParamsAction<_,_>>)>]
type ThisParamsActionProxy<'TThis, 'TParams> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Action<'TThis, 'TParams>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsAction<'TParams>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ThisParamsAction<_,_,_>>)>]
type ThisParamsActionProxy<'TThis, 'T, 'TParams> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Action<'TThis, 'T, 'TParams>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsAction<'T, 'TParams>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg: 'T, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ThisParamsAction<_,_,_,_>>)>]
type ThisParamsActionProxy<'TThis, 'T1, 'T2, 'TParams> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Action<'TThis, 'T1, 'T2, 'TParams>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsAction<'T1, 'T2, 'TParams>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ThisParamsAction<_,_,_,_,_>>)>]
type ThisParamsActionProxy<'TThis, 'T1, 'T2, 'T3, 'TParams> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Action<'TThis, 'T1, 'T2, 'T3, 'TParams>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsAction<'T1, 'T2, 'T3, 'TParams>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ThisParamsAction<_,_,_,_,_,_>>)>]
type ThisParamsActionProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'TParams> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Action<'TThis, 'T1, 'T2, 'T3, 'T4, 'TParams>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsAction<'T1, 'T2, 'T3, 'T4, 'TParams>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ThisParamsAction<_,_,_,_,_,_,_>>)>]
type ThisParamsActionProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'TParams> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Action<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'TParams>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsAction<'T1, 'T2, 'T3, 'T4, 'T5, 'TParams>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ThisParamsAction<_,_,_,_,_,_,_,_>>)>]
type ThisParamsActionProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TParams> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Action<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TParams>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsAction<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TParams>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5, arg6: 'T6, [<PA>] rest: 'TParams[]) = X<unit>
[<Proxy (typeof<ThisParamsFunc<_,_,_>>)>]
type ThisParamsFuncProxy<'TThis, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Func<'TThis, 'TParams, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsFunc<'TParams, 'TResult>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ThisParamsFunc<_,_,_,_>>)>]
type ThisParamsFuncProxy<'TThis, 'T, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Func<'TThis, 'T, 'TParams, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsFunc<'T, 'TParams, 'TResult>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg: 'T, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ThisParamsFunc<_,_,_,_,_>>)>]
type ThisParamsFuncProxy<'TThis, 'T1, 'T2, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Func<'TThis, 'T1, 'T2, 'TParams, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsFunc<'T1, 'T2, 'TParams, 'TResult>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ThisParamsFunc<_,_,_,_,_,_>>)>]
type ThisParamsFuncProxy<'TThis, 'T1, 'T2, 'T3, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Func<'TThis, 'T1, 'T2, 'T3, 'TParams, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsFunc<'T1, 'T2, 'T3, 'TParams, 'TResult>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ThisParamsFunc<_,_,_,_,_,_,_>>)>]
type ThisParamsFuncProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Func<'TThis, 'T1, 'T2, 'T3, 'T4, 'TParams, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsFunc<'T1, 'T2, 'T3, 'T4, 'TParams, 'TResult>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ThisParamsFunc<_,_,_,_,_,_,_,_>>)>]
type ThisParamsFuncProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Func<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'TParams, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsFunc<'T1, 'T2, 'T3, 'T4, 'T5, 'TParams, 'TResult>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<ThisParamsFunc<_,_,_,_,_,_,_,_,_>>)>]
type ThisParamsFuncProxy<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TParams, 'TResult> =
    [<Inline "$wsruntime.ThisParamsFunc($del)">]
    new (del: System.Func<'TThis, 'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TParams, 'TResult>) = { }
    [<Inline "$this.bind($thisArg)">]
    member this.Bind(thisArg: 'TThis) = X<ParamsFunc<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'TParams, 'TResult>>
    [<Macro(typeof<Macro.JSThisParamsCall>)>]
    member this.Call(thisArg: 'TThis, arg1: 'T1, arg2: 'T2, arg3: 'T3, arg4: 'T4, arg5: 'T5, arg6: 'T6, [<PA>] rest: 'TParams[]) = X<'TResult>
[<Proxy (typeof<Union<_,_>>)>]
type UnionProxy<'T1, 'T2> =
    | Union1Of2 of 'T1
    | Union2Of2 of 'T2
    [<Inline>]
    member this.Value1 = As<'T1> this
    [<Inline>]
    member this.Value2 = As<'T2> this
[<Proxy (typeof<Union<_,_,_>>)>]
type UnionProxy<'T1, 'T2, 'T3> =
    | Union1Of3 of 'T1
    | Union2Of3 of 'T2
    | Union3Of3 of 'T3
    [<Inline>]
    member this.Value1 = As<'T1> this
    [<Inline>]
    member this.Value2 = As<'T2> this
    [<Inline>]
    member this.Value3 = As<'T3> this
[<Proxy (typeof<Union<_,_,_,_>>)>]
type UnionProxy<'T1, 'T2, 'T3, 'T4> =
    | Union1Of4 of 'T1
    | Union2Of4 of 'T2
    | Union3Of4 of 'T3
    | Union4Of4 of 'T4
    [<Inline>]
    member this.Value1 = As<'T1> this
    [<Inline>]
    member this.Value2 = As<'T2> this
    [<Inline>]
    member this.Value3 = As<'T3> this
    [<Inline>]
    member this.Value4 = As<'T4> this
[<Proxy (typeof<Union<_,_,_,_,_>>)>]
type UnionProxy<'T1, 'T2, 'T3, 'T4, 'T5> =
    | Union1Of5 of 'T1
    | Union2Of5 of 'T2
    | Union3Of5 of 'T3
    | Union4Of5 of 'T4
    | Union5Of5 of 'T5
    [<Inline>]
    member this.Value1 = As<'T1> this
    [<Inline>]
    member this.Value2 = As<'T2> this
    [<Inline>]
    member this.Value3 = As<'T3> this
    [<Inline>]
    member this.Value4 = As<'T4> this
    [<Inline>]
    member this.Value5 = As<'T5> this
[<Proxy (typeof<Union<_,_,_,_,_,_>>)>]
type UnionProxy<'T1, 'T2, 'T3, 'T4, 'T5, 'T6> =
    | Union1Of6 of 'T1
    | Union2Of6 of 'T2
    | Union3Of6 of 'T3
    | Union4Of6 of 'T4
    | Union5Of6 of 'T5
    | Union6Of6 of 'T6
    [<Inline>]
    member this.Value1 = As<'T1> this
    [<Inline>]
    member this.Value2 = As<'T2> this
    [<Inline>]
    member this.Value3 = As<'T3> this
    [<Inline>]
    member this.Value4 = As<'T4> this
    [<Inline>]
    member this.Value5 = As<'T5> this
    [<Inline>]
    member this.Value6 = As<'T6> this
[<Proxy (typeof<Union<_,_,_,_,_,_,_>>)>]
type UnionProxy<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'T7> =
    | Union1Of7 of 'T1
    | Union2Of7 of 'T2
    | Union3Of7 of 'T3
    | Union4Of7 of 'T4
    | Union5Of7 of 'T5
    | Union6Of7 of 'T6
    | Union7Of7 of 'T7
    [<Inline>]
    member this.Value1 = As<'T1> this
    [<Inline>]
    member this.Value2 = As<'T2> this
    [<Inline>]
    member this.Value3 = As<'T3> this
    [<Inline>]
    member this.Value4 = As<'T4> this
    [<Inline>]
    member this.Value5 = As<'T5> this
    [<Inline>]
    member this.Value6 = As<'T6> this
    [<Inline>]
    member this.Value7 = As<'T7> this
// }}
