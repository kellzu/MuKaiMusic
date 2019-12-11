import { Component, OnInit } from '@angular/core';
import { ThemeService } from './services/theme/theme.service';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  ngOnInit(): void {
    this.router.events.subscribe((ev) => {
      //暂存content面板中打开页面的URL
      if (ev instanceof NavigationEnd) {
        if (ev.url.includes('/content/'))
          this._url_save = ev.url;
      }
    });
  }
  title = 'Mukai Music';
  public _showContent: boolean = false;

  public _url_save: string = '/content/explore';

  public get contentOpened() {
    return location.href.includes('/content');
  }

  constructor(private theme: ThemeService,
    private router: Router) {
  }
  public get themeClass(): string {
    return this.theme.getThemeClass();
  }

  //打开content面板
  public showContent() {
    if (!this.contentOpened) {
      this.router.navigate([this._url_save]);
    } else {
      this.router.navigate(['/']);
    }
  }
}
