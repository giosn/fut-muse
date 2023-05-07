import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'circle-img[imageUrl][size]',
    templateUrl: './circle-img.component.html',
    styleUrls: ['./circle-img.component.scss']
})
export class CircleImgComponent implements OnInit {

    @Input('imageUrl') imageUrl: string;
    @Input('size') size: number | string;
    @Input('fit') fit: 'cover' | 'contain' = 'cover';
    @Input('right') right = 0;
    @Input('noBorder') noBorder = false;
    @Input('noRadius') noRadius = false;
    
    constructor() { }

    trueSize: string;

    ngOnInit(): void {
        this.trueSize = typeof this.size === 'number' ? `${this.size}px`: this.size;
        this.imageUrl.replace('/big/', '/medium/')
    }

    handleImgError(event: Event) {
        if (this.imageUrl.includes('/big/')) {
            const img = event.target as HTMLImageElement;
            img.src = this.imageUrl.replace('/big/', '/medium/');
        }
    }

}
