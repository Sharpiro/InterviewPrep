﻿import {Pipe, PipeTransform} from "angular2/core"

@Pipe({ name: "CustomPipe" })
export class CustomPipe implements PipeTransform
{
    public transform(value: string, args: any[]): string
    {
        return "this value has been transformed into me";
    }
}