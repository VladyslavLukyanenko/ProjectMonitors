import {NgModule} from "@angular/core";
import {SharedModule} from "../shared/shared.module";
import {AdminRoutesModule} from "./admin-routes.module";
import {TranslateLoader, TranslateModule} from "@ngx-translate/core";
import {httpLoaderFactory} from "../ngx-translate-http-loader";
import {HttpClient} from "@angular/common/http";
import {DashboardPageComponent} from "./components/dashboard-page/dashboard-page.component";
import {ServerImagesPageComponent} from "./components/server-images-page/server-images-page.component";
import {AddMonitorDialogComponent} from "./components/add-monitor-dialog/add-monitor-dialog.component";
import {SpawnMonitorDialogComponent} from "./components/spawn-monitor-dialog/spawn-monitor-dialog.component";
import {AddServerNodeDialogComponent} from "./components/add-server-node-dialog/add-server-node-dialog.component";
import {NodeRuntimeInfoComponent} from "./components/node-runtime-info/node-runtime-info.component";


@NgModule({
  declarations: [
    ServerImagesPageComponent,
    DashboardPageComponent,
    AddMonitorDialogComponent,
    SpawnMonitorDialogComponent,
    AddServerNodeDialogComponent,
    NodeRuntimeInfoComponent
  ],
  imports: [
    SharedModule,
    AdminRoutesModule,

    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useFactory: httpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ]
})
export class AdminModule { }
