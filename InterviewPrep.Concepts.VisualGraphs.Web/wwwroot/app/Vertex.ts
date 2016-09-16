﻿class Vertex
{
    public neighbors: Neighbor[];
    public isActive: boolean;
    public progress: number;
    public imageUrl: string;
    public image: HTMLImageElement;
    private colorSelectorFactory: PathColorFactory;

    constructor(public name: string, public position: Rectangle = null)
    {
        this.neighbors = [];
        this.progress = 0;
        this.colorSelectorFactory = new PathColorFactory();
    }

    public update(): void
    {
        this.updateActive();
    }

    public updateActive(): void
    {
        for (var neighbor of this.neighbors)
        {
            if (neighbor.vertex.isFull())
            {
                this.isActive = true;
                break;
            }
        }
    }

    public getWeight(): number
    {
        return this.neighbors[0].weight;
    }

    public activate(): void
    {
        if (this.isFull()) return;
        if (!this.isActive) return;
        this.progress++;
    }

    public activateFull(): void
    {
        if (this.isFull()) return;
        this.isActive = true;
        this.progress = this.getWeight();
    }

    public deactivateFull(): void
    {
        if (this.isEmpty()) return;
        this.isActive = false;
        this.progress = 0;
    }

    public deactivate(): void
    {
        if (this.isEmpty()) return;
        //this.updateActive();
        this.progress--;
    }

    public isFull(): boolean
    {
        return this.isActive && this.progress == this.neighbors[0].weight;
    }

    public isEmpty(): boolean
    {
        return this.isActive && this.progress == 0;
    }

    public drawConnections(context: CanvasRenderingContext2D, number: number): void
    {
        var midX = this.position.x + this.position.width / 2;
        var midY = this.position.y + this.position.height / 2;
        context.lineWidth = 7;
        var colorSelector = this.colorSelectorFactory.getDrawer(number);
        for (var neighbor of this.neighbors)
        {
            context.strokeStyle = colorSelector.selectColor(this, neighbor);
            var neighborX = neighbor.vertex.position.x + neighbor.vertex.position.width / 2;
            var neighborY = neighbor.vertex.position.y + neighbor.vertex.position.height / 2;
            context.beginPath();
            context.moveTo(midX, midY);
            context.lineTo(neighborX, neighborY);
            context.stroke();
            context.closePath();
        }
    }

    public draw(context: CanvasRenderingContext2D): void
    {
        var midX = this.position.x + this.position.width / 3;
        var midY = this.position.y + this.position.height / 1.5;
        if (!this.image)
        {
            this.image = new Image();
            this.image.src = this.imageUrl;
        }
        if (this.isFull())
        {
            context.drawImage(this.image, this.position.x, this.position.y, this.position.width, this.position.height);
        }
        else
        {
            context.drawImage(this.image, this.position.x, this.position.y, this.position.width, this.position.height);
            context.fillStyle = "rgba(0, 0, 0, 0.6)";
            context.fillRect(this.position.x, this.position.y, this.position.width, this.position.height);
        }
        context.fillStyle = "white";
        context.font = "30px Consolas"
        context.fillText(this.name, midX, midY);
        context.font = "15px Consolas"
        context.fillStyle = "black";
        context.fillText(`${this.progress}/${this.getWeight()}`, midX, this.position.y + this.position.height + 15);
    }
}