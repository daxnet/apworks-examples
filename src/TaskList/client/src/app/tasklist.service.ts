import { Injectable } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { RequestOptionsBuilder } from './http.builder';
import { TaskItem } from './taskitem';
import { TaskListResponse } from './tasklist.response';
import { environment } from '../environments/environment';

const PAGE_SIZE = 5;

@Injectable()
export class TaskListService {

    private headers = new Headers({ 'Content-Type': 'application/json' });

    constructor(private http: Http) {

    }

    getTaskList(pageIndex: number = 1): Promise<TaskListResponse> {
        const url = `${environment.serviceBaseUrl}?size=${PAGE_SIZE}&page=${pageIndex}`;
        return this.http.get(url)
            .toPromise()
            .then(response => {
                let json = response.json();
                return new TaskListResponse(json.pageNumber,
                    json.pageSize,
                    json.totalRecords,
                    json.totalPages,
                    json._embedded.taskitems as TaskItem[]);
            })
            .catch(this.handleError);
    }

    updateItemStatus(item: TaskItem): void {
        const url = `${environment.serviceBaseUrl}/${item.Id}`;
        var body = JSON.stringify([{
            op: "replace",
            path: "/Done",
            value: item.Done
        }]);

        var builder = new RequestOptionsBuilder();
        builder.withHeader('Content-Type', 'application/json');
        this.http.patch(url, body, builder.build())
            .toPromise()
            .catch(this.handleError);
    }

    addNew(title: string): Promise<string> {
        const url = `${environment.serviceBaseUrl}`;
        var item = new TaskItem();
        item.Title = title;
        var body = JSON.stringify(item);
        var builder = new RequestOptionsBuilder();
        builder.withHeader('Content-Type', 'application/json');
        return this.http.post(url, body, builder.build())
            .toPromise()
            .then(response => response.toString())
            .catch(this.handleError);
    }

    updateAllStatus(done: boolean): Promise<any> {
        const url = `${environment.serviceBaseUrl}/all`;
        var body = JSON.stringify(done);

        var builder = new RequestOptionsBuilder();
        builder.withHeader('Content-Type', 'application/json');
        return this.http.post(url, body, builder.build())
            .toPromise()
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred.', error);
        return Promise.reject(error.message || error);
    }
}