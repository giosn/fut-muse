import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'circle-img[imageUrl][size]',
    templateUrl: './circle-img.component.html',
    styleUrls: ['./circle-img.component.scss']
})
export class CircleImgComponent implements OnInit {

    @Input() imageUrl: string;
    @Input() size: number | string;
    @Input() fit: 'cover' | 'contain' = 'cover';
    @Input() right = 0;
    @Input() noBorder = false;
    @Input() noRadius = false;
    
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
