import { CourseCheckBox } from "./courseCheckBox.model";

export interface Student{
    id:number;
    name:string;
    regNo:number;
    dateOfbirth: Date;
    courseCheckBoxList: CourseCheckBox[]
}