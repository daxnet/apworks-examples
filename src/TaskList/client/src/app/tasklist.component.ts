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

import { Component, OnInit } from '@angular/core';

import { TaskItem } from './taskitem';
import { TaskListService } from './tasklist.service';
import { TaskListResponse } from './tasklist.response';
import { NotificationsService } from 'angular2-notifications';

@Component({
    selector: 'task-list',
    templateUrl: './tasklist.component.html',
    styleUrls: ['./tasklist.component.css'],
    providers: [TaskListService]
})

export class TaskListComponent implements OnInit {

    taskListResponse: TaskListResponse;

    // The number array which holds the indecies of each page.
    pageIndecies: number[] = new Array();

    // The number which holds the current page number.
    currentPageIdx: number;

    newTaskItemTitle: string;

    hideRequiredMessage: boolean = true;

    ngOnInit(): void {
        this.getTaskList();
    }

    constructor(private service: TaskListService, private notification: NotificationsService) {

    }

    private getTaskList(): void {
        this.service.getTaskList()
            .then(response => {
                this.taskListResponse = response;
                console.log(`Total Records: ${response.totalRecords}, Total Pages: ${response.totalPages} `);
                for (var idx = 1; idx <= response.totalPages; idx++) {
                    this.pageIndecies[idx - 1] = idx;
                }
                this.currentPageIdx = response.totalPages > 0 ? 1 : 0;
            }).catch((err) => {
                this.notification.error('Error', err, notificationOptions);
            });
    }

    private onStatusUpdated(item): void {
        this.service.updateItemStatus(item)
            .then(response => this.notification.success('Success', 'Status updated successfully!', notificationOptions))
            .catch((err) => {
                this.notification.error('Error', err, notificationOptions);
            });
    }

    private onPageSelected(idx): void {
        this.service.getTaskList(idx)
            .then(response => {
                this.taskListResponse = response;
                this.currentPageIdx = idx;
            }).catch((err) => {
                this.notification.error('Error', err, notificationOptions);
            });
    }

    onAddNewClick(): void {
        this.hideRequiredMessage = this.newTaskItemTitle != null && this.newTaskItemTitle != '';
        if (this.newTaskItemTitle) {
            this.service.addNew(this.newTaskItemTitle)
                .then(response => {
                    this.getTaskList();
                    this.newTaskItemTitle = '';
                    this.hideRequiredMessage = true;
                    this.notification.success('Success', 'New task has been added successfully!', notificationOptions);
                }).catch((err) => {
                    this.notification.error('Error', err, notificationOptions);
                });

        }
    }

    private onUpdateAllStatusClick(done: boolean): void {
        this.service.updateAllStatus(done)
            .then(response => {
                this.getTaskList();
                this.notification.success('Success', 'Status updated successfully!', notificationOptions);
            }).catch((err) => {
                this.notification.error('Error', err, notificationOptions);
            });
    }
}

const notificationOptions: any = {
    timeOut: 8000,
    showProgressBar: true,
    pauseOnHover: true,
    clickToClose: true,
    maxLength: 50
};