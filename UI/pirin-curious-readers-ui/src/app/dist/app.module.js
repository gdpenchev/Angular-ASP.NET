"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
exports.__esModule = true;
exports.AppModule = void 0;
var ngx_pagination_1 = require("ngx-pagination");
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var http_1 = require("@angular/common/http");
var app_routing_module_1 = require("./app-routing.module");
var ngx_toastr_1 = require("ngx-toastr");
var app_component_1 = require("./app.component");
var side_menu_component_1 = require("./components/side-menu/side-menu.component");
var login_component_1 = require("./components/login/login.component");
var auth_service_1 = require("./services/auth.service");
var animations_1 = require("@angular/platform-browser/animations");
var forms_1 = require("@angular/forms");
var signUp_component_1 = require("./components/register/signUp.component");
var add_book_component_1 = require("./components/add-book/add-book.component");
var ng_select_1 = require("@ng-select/ng-select");
var ng_multiselect_dropdown_1 = require("ng-multiselect-dropdown");
var authentication_guard_1 = require("./services/authentication.guard");
var authentication_interceptor_1 = require("./services/authentication.interceptor");
var home_component_1 = require("./components/home/home.component");
var librarian_hub_component_1 = require("./components/librarian-hub/librarian-hub.component");
var angular_jwt_1 = require("@auth0/angular-jwt");
var books_component_1 = require("./components/books/books.component");
var card_1 = require("@angular/material/card");
var input_1 = require("@angular/material/input");
var button_1 = require("@angular/material/button");
var icon_1 = require("@angular/material/icon");
var book_card_component_1 = require("./components/books/book-card/book-card.component");
var forms_2 = require("@angular/forms");
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            declarations: [
                app_component_1.AppComponent,
                side_menu_component_1.SideMenuComponent,
                login_component_1.LoginComponent,
                signUp_component_1.SignUpComponent,
                add_book_component_1.AddBookComponent,
                home_component_1.HomeComponent,
                librarian_hub_component_1.LibrarianHubComponent,
                books_component_1.BooksComponent,
                book_card_component_1.BookCardComponent
            ],
            imports: [
                platform_browser_1.BrowserModule,
                app_routing_module_1.AppRoutingModule,
                forms_1.ReactiveFormsModule,
                http_1.HttpClientModule,
                animations_1.BrowserAnimationsModule,
                ngx_toastr_1.ToastrModule.forRoot({
                    progressBar: true,
                    closeButton: true,
                    preventDuplicates: true
                }),
                ng_select_1.NgSelectModule,
                ng_multiselect_dropdown_1.NgMultiSelectDropDownModule.forRoot(),
                http_1.HttpClientModule,
                angular_jwt_1.JwtModule.forRoot({
                    config: {
                        tokenGetter: function () { return localStorage.getItem('token'); }
                    }
                }),
                card_1.MatCardModule,
                input_1.MatInputModule,
                button_1.MatButtonModule,
                icon_1.MatIconModule,
                forms_2.FormsModule,
                ngx_pagination_1.NgxPaginationModule,
            ],
            providers: [auth_service_1.AuthService,
                authentication_guard_1.AuthenticationGuard,
                { provide: http_1.HTTP_INTERCEPTORS, useClass: authentication_interceptor_1.AuthenticationInterceptor, multi: true }],
            bootstrap: [app_component_1.AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
