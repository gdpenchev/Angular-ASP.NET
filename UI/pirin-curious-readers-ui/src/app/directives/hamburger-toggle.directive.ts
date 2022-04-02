import { Directive, HostBinding, HostListener } from '@angular/core';

@Directive({
  selector: '[appHamburgerToggle]'
})
export class HamburgerToggleDirective {
  @HostBinding('class.is-active')
  private isActive = false;

  @HostListener('click')
  toggleActive(): void {
    this.isActive = !this.isActive;    
  }

  @HostListener('window:resize', ['$event'])
    onResize(): void {
      this.isActive = window.innerWidth >= 980 ? true : false;
  }
}