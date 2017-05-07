// ==================================================================================================================                                                                                          
//        ,::i                                                           BBB                
//       BBBBBi                                                         EBBB                
//      MBBNBBU                                                         BBB,                
//     BBB. BBB     BBB,BBBBM   BBB   UBBB   MBB,  LBBBBBO,   :BBG,BBB :BBB  .BBBU  kBBBBBF 
//    BBB,  BBB    7BBBBS2BBBO  BBB  iBBBB  YBBJ :BBBMYNBBB:  FBBBBBB: OBB: 5BBB,  BBBi ,M, 
//   MBBY   BBB.   8BBB   :BBB  BBB .BBUBB  BB1  BBBi   kBBB  BBBM     BBBjBBBr    BBB1     
//  BBBBBBBBBBBu   BBB    FBBP  MBM BB. BB BBM  7BBB    MBBY .BBB     7BBGkBB1      JBBBBi  
// PBBBFE0GkBBBB  7BBX   uBBB   MBBMBu .BBOBB   rBBB   kBBB  ZBBq     BBB: BBBJ   .   iBBB  
//BBBB      iBBB  BBBBBBBBBE    EBBBB  ,BBBB     MBBBBBBBM   BBB,    iBBB  .BBB2 :BBBBBBB7  
//vr7        777  BBBu8O5:      .77r    Lr7       .7EZk;     L77     .Y7r   irLY  JNMMF:    
//               LBBj
//
// Apworks Application Development Framework
// Copyright (C) 2009-2017 by daxnet.
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ==================================================================================================================

import { Injectable } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { RequestOptionsBuilder } from './http.builder';
import { TaskItem } from './taskitem';
import { TaskListResponse } from './tasklist.response';
import { environment } from '../environments/environment';


@Injectable()
export class TaskListService {

    private headers = new Headers({ 'Content-Type': 'application/json' });

    constructor(private http: Http) {

    }

    getTaskList(pageIndex: number = 1): Promise<TaskListResponse> {
        const url = `${environment.serviceBaseUrl}?size=${environment.pageSize}&page=${pageIndex}`;
        return this.http.get(url)
            .toPromise()
            .then(response => {
                let json = response.json();
                return new TaskListResponse(json.pageNumber,
                    json.pageSize,
                    json.totalRecords,
                    json.totalPages,
                    json._embedded.taskitems as TaskItem[]);
            }).catch((err) => {throw err;});
    }

    updateItemStatus(item: TaskItem): Promise<any> {
        const url = `${environment.serviceBaseUrl}/${item.Id}`;
        var body = JSON.stringify([{
            op: "replace",
            path: "/Done",
            value: item.Done
        }]);

        var builder = new RequestOptionsBuilder();
        builder.withHeader('Content-Type', 'application/json');
        return this.http.patch(url, body, builder.build())
            .toPromise()
            .catch((err) => {throw err;});
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
            .catch((err) => {throw err;});
    }

    updateAllStatus(done: boolean): Promise<any> {
        const url = `${environment.serviceBaseUrl}/all`;
        var body = JSON.stringify(done);

        var builder = new RequestOptionsBuilder();
        builder.withHeader('Content-Type', 'application/json');
        return this.http.post(url, body, builder.build())
            .toPromise()
            .catch((err) => {throw err;});
    }
}