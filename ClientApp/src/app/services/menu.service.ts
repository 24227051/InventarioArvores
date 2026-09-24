import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MenuService {
  private _showMenu = new BehaviorSubject<boolean>(true);
  showMenu$ = this._showMenu.asObservable();

  setShowMenu(value: boolean) {
    this._showMenu.next(value);
  }
}
