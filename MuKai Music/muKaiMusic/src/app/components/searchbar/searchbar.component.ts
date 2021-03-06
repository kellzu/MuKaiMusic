import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ThemeService } from 'src/app/services/theme/theme.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
@Component({
  selector: 'app-searchbar',
  templateUrl: './searchbar.component.html',
  styleUrls: ['./searchbar.component.scss']
})
export class SearchbarComponent implements OnInit {

  constructor(
    private theme: ThemeService,
    private router: Router) {
    this.router.events.subscribe(ev => {
      if (ev instanceof NavigationEnd) {
        if (ev.url.includes('/search/')) {
          let s: string = ev.url.substring(ev.url.indexOf('/search/') + 8);
          this._value = decodeURI(decodeURI(s));
        }
      }
    });
  }

  ngOnInit(): void {
    let search = document.getElementById("search");
    search.onfocus = () => {
      window.onkeypress = (ev: KeyboardEvent) => {
        if (ev.key == "Enter" && this.value != "") {
          this.inputComplete.emit(this.value);
        }
      }
    }
    search.onblur = () => {
      window.onkeypress = null;
    }
    // this.route.paramMap.subscribe(map => {
    //   console.log(map);
    //   this._value = decodeURI(map.get("key"));
    // });

  }

  private _placeholder: string = "";
  @Input()
  public get placeholder(): string {
    return this._placeholder;
  }
  public set placeholder(value: string) {
    this._placeholder = value;
  }

  /**
   * 与输入框的值进行绑定
   */
  @Input()
  public get value(): string {
    return this._value;
  }
  public set value(value: string) {
    this._value = value;
  }
  private _value: string = "";

  public get themeClass(): string {
    return this.theme.getThemeClass();
  }

  public iconClick() {
    if (this.value == "") {
      return;
    }
    this.inputComplete.emit(this.value);
  }
  /**
   * 输入完毕事件，当点击放大镜图标
   * 或者在input框处于focus状态下按下Enter键时触发
   */
  @Output()
  public inputComplete = new EventEmitter<string>();
}
