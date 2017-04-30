import { Component, OnInit } from '@angular/core';

import { TaskItem } from './taskitem';
import { TaskListService } from './tasklist.service';
import { TaskListResponse } from './tasklist.response';

@Component ({
    selector: 'task-list',
    templateUrl: './tasklist.component.html',
    styleUrls: ['./tasklist.component.css'],
    providers: [TaskListService]
})

export class TaskListComponent implements OnInit {

    private taskListResponse: TaskListResponse;
    private serviceUrl = "http://localhost:39518/api/taskItems";
    
    // The number array which holds the indecies of each page.
    private pageIndecies: number[] = new Array();

    // The number which holds the current page number.
    private currentPageIdx: number;

    ngOnInit(): void {
        this.getTaskList();
        
    }

    constructor (private service: TaskListService) {

    }

    private getTaskList(): void {
        this.service.getTaskList()
            .then(response => {
                this.taskListResponse = response;
                for(var idx = 1; idx <= response.totalPages; idx++) {
                    this.pageIndecies[idx - 1] = idx;
                }
                this.currentPageIdx = response.totalPages > 0 ? 1 : 0;
        });
    }

    private onStatusUpdated(item): void {
        this.service.updateItemStatus(item);
    }

    private onPageSelected(idx): void {
        this.service.getTaskList(idx)
            .then(response => {
                this.taskListResponse = response;
                this.currentPageIdx = idx;
            });
    }
}