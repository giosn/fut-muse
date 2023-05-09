import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { By } from '@angular/platform-browser';
import { Router, RouterLinkWithHref } from '@angular/router';

describe('AppComponent', () => {
    let fixture: ComponentFixture<AppComponent>;
    let router: Router;
    let routerSpy: jasmine.Spy;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [ RouterTestingModule ],
            declarations: [ AppComponent ],
        }).compileComponents();

        fixture = TestBed.createComponent(AppComponent);
        router = TestBed.inject(Router);
        routerSpy = spyOn(router, 'navigateByUrl');
    });

    it('should navigate to the default page on logo click', () => {
        const logo = fixture.debugElement.query(By.directive(RouterLinkWithHref));
        expect(logo).not.toBeNull();
        logo.nativeElement.click();
        expect(routerSpy).toHaveBeenCalledWith(
            router.createUrlTree(['/']), 
            jasmine.anything()
        );
    });

});
