import { TaskItem } from './taskitem';

export class TaskListResponse {

    constructor (public pageNumber: number,
        public pageSize: number,
        public totalRecords: number,
        public totalPages: number,
        public taskItems: TaskItem[]) {
    }
}
