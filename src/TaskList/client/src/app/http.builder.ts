/**
 * A fluent builder to construct a `RequestOptions` object.
 *
 * ### Example
 *
 * ```typescript
 * let options = RequestOptions.builder()
 *     .withHeader('Accept', 'application/json')
 *     .withHeader('Authorization', `Bearer: ${token}`
 *     .withParam('question', question)
 *     .withParam('option', 'fast')
 *     .build();
 * ```
 */
import { RequestOptions, URLSearchParams, Headers, RequestMethod } from '@angular/http';

export class RequestOptionsBuilder {

  private options: RequestOptions;

  constructor() {
    let options = new RequestOptions();
    options.search = new URLSearchParams();
    options.headers = new Headers();
    this.options = options;
  }

  /**
   * Sets the URL (replaces the previous one, if any)
   * @param url the URL to set in the request options
   * @returns {RequestOptionsBuilder} this builder, for chaining
   */
  withUrl(url: string): RequestOptionsBuilder {
    this.options.url = url;
    return this;
  }

  /**
   * Sets the method (replaces the previous one, if any)
   * @param method the method to set in the request options
   * @returns {RequestOptionsBuilder} this builder, for chaining
   */
  withMethod(method: RequestMethod | string): RequestOptionsBuilder {
    this.options.method = method;
    return this;
  }

  /**
   * Appends the given header
   * @param header the name of the header to append in the request options
   * @param value the value of the header to append in the request options
   * @returns {RequestOptionsBuilder} this builder, for chaining
   */
  withHeader(header: string, value: string): RequestOptionsBuilder {
    this.options.headers.append(header, value);
    return this;
  }

  /**
   * Sets (or replaces) the given header with the given values
   * @param header the name of the header to set in the request options
   * @param newValues the values for the header to set in the request options
   * @returns {RequestOptionsBuilder} this builder, for chaining
   */
  withHeaders(header: string, newValues: string[]): RequestOptionsBuilder {
    this.options.headers.set(header, newValues);
    return this;
  }

  /**
   * Sppends the given search parameter
   * @param param the name of the parameter to append in the request options
   * @param value the value of the parameter to append in the request options
   * @returns {RequestOptionsBuilder} this builder, for chaining
   */
  withParam(param: string, value: string): RequestOptionsBuilder {
    this.options.search.append(param, value);
    return this;
  }

  /**
   * Sets (or replaces) the given search parameter with the given values
   * @param param the name of the parameter to set in the request options
   * @param newValues the values for the parameter to set in the request options
   * @returns {RequestOptionsBuilder} this builder, for chaining
   */
  withParams(param: string, newValues: string[]): RequestOptionsBuilder {
    this.options.search.delete(param);
    newValues.forEach(value => this.options.search.append(param, value));
    return this;
  }

  /**
   * Sets (or replaces) the `withCredentials` flag with the given value
   * @param withCredentials the new value of the flag
   * @returns {RequestOptionsBuilder} this builder, for chaining
   */
  withCredentials(withCredentials: boolean): RequestOptionsBuilder {
    this.options.withCredentials = withCredentials;
    return this;
  }

  /**
   * Builds the request options
   * @returns {RequestOptions}
   */
  build(): RequestOptions {
    return this.options;
  }
}