﻿import {Component, provide, OpaqueToken} from "angular2/core"
import {RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS } from "angular2/router"
import {InMemoryBackendService, SEED_DATA}  from 'a2-in-memory-web-api/core';
import {InMemoryDb} from "../fakeApi/inMemoryDb"
import {HTTP_PROVIDERS, XHRBackend} from "angular2/http"
import {DashboardComponent, StaticVehicleService, VehicleService, VehiclesComponent,
VehicleListComponent, VehicleComponent, SpinnerComponent, SpinnerService} from "./appCore"

@Component({
    selector: "my-app",
    directives: [ROUTER_DIRECTIVES, DashboardComponent, SpinnerComponent],
    providers: [HTTP_PROVIDERS, provide("IVehicleServiceToken", { useClass: StaticVehicleService }),
        provide(XHRBackend, { useClass: InMemoryBackendService }),
        provide(SEED_DATA, { useClass: InMemoryDb }),
        SpinnerService],
    templateUrl: "./app/appComponent.html",
    styles: [
        `nav ul {list-style-type: none;}
        nav ul li {padding: 4px;display:inline-block}`
    ]
})
@RouteConfig([
    { path: "/dashboard", name: "Dashboard", component: DashboardComponent, useAsDefault: true },
    { path: "/vehicles/...", name: "Vehicles", component: VehiclesComponent }
])
export class AppComponent
{
    public changed(changedCharacter: IBaseData): void
    {
        const message = `Event changed: ${changedCharacter.name}`;
        toastr.success(message);
        console.log(message);
    }
}