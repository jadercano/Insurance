import { Injectable } from '@angular/core';
import { BaseRequestOptions, RequestOptions, RequestOptionsArgs, Headers } from '@angular/http';
import { environment } from '../../environments/environment'

@Injectable()
export class CustomRequestOptions extends BaseRequestOptions {
  merge(options?: RequestOptionsArgs): RequestOptions {
    options.url = environment.base_url + options.url;
    var result = super.merge(options);
    result.merge = this.merge;
    return result;
  }
}
