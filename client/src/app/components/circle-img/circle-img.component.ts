import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'circle-img[imageUrl][size]',
    templateUrl: './circle-img.component.html',
    styleUrls: ['./circle-img.component.scss']
})
export class CircleImgComponent implements OnInit {

    @Input('imageUrl') imageUrl: string;
    @Input('size') size: number;
    @Input('fit') fit: 'cover' | 'contain' = 'cover';
    @Input('right') right = 0;
    @Input('noBorder') noBorder = false;
    @Input('noRadius') noRadius = false;
    
    constructor() { }

    ngOnInit(): void {
    }

}
