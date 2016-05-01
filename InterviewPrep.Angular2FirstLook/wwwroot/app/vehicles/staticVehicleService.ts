﻿import {Injectable} from 'angular2/core';
import {Http, Response} from 'angular2/http';
import {Observable, Operator} from "rxjs/Rx"
import {IVehicleService} from "./vehicles"

@Injectable()
export class StaticVehicleService implements IVehicleService
{
    constructor(private httpService: Http) { }

    public getVehicles(): Observable<IBaseData[]>
    {
        var promise = this.httpService.get("/api/vehicles")
            .map((response: Response) => <IBaseData[]>(response.json().data))
            .do(data => console.log())
            .catch(this.handleError);

        //var promise2 = Observable.create((observer: any) =>
        //{
        //    var data: IBaseData[] = [{ id: 1, name: "whatever", type: "space" }, { id: 2, name: "whatever2", type: "space" }];
        //    observer.next(data);
        //    observer.complete();
        //});
        return promise;
    }

    public getVehicle(id: number): Observable<IBaseData>
    {
        var promise = this.getVehicles().map(vehicles => vehicles.find(vehicle => vehicle.id === id));
        return promise;
    }

    private handleError(error: Response): Observable<any>
    {
        console.error(error);
        var obs = Observable.throw("Server Error");
        return obs;
    }
}