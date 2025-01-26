
window.profileCanvas = {
    targetId: "profileCanvas",
    ctx: null,
    canvas: null,
    container: null,
    profiles: null,
    markerWidth: 3,
    lineWidth: 1,

    link: function () {
        // Ignore repeats
        if (this.ctx != null) return;

        // Try link
        this.canvas = document.getElementById(this.targetId);
        this.container = document.getElementById(this.targetId + 'Container');

        if (this.canvas != null && this.container != null) {
            // Link context and listen for resize events
            this.ctx = this.canvas.getContext('2d');
            window.addEventListener('resize', () => { this.draw() });
        }
        else {
            console.error(`Linking #${this.targetId} failed`);
        }
    },

    set: function (_profiles) {
        this.profiles = _profiles;
        this.draw();
    },

    draw: function () {
        // Scaling tips & tricks: https://joshondesign.com/2023/04/15/canvas_scale_smooth and MDN

        // Null colase canvas context
        if (this.ctx == null) {
            console.warn(`Canvas #${this.targetId} not linked`);
            return;
        }

        // Set canvas size and scale
        let bounds = this.container.getBoundingClientRect();
        let pxScale = window.devicePixelRatio;
        let width = Math.floor(bounds.width);
        let height = Math.floor(bounds.height);

        this.canvas.style.width = `${width}px`;
        this.canvas.style.height = `${height}px`;
        this.canvas.width = width * pxScale;
        this.canvas.height = height * pxScale;
        this.ctx.scale(pxScale, pxScale);

        // Clear canvas
        this.ctx.clearRect(0, 0, width, height);

        // Calculate Scales and offsets
        let scale = width / 1.2;
        let offsetX = width / 12;
        let offsetY = height / 2;
        let markerOffset = this.markerWidth / 2 - 0.5;

        // Check profile existance
        if (this.profiles != null && this.profiles.length > 0) {
            for (i = 0; i < this.profiles.length; i++) {
                let profile = this.profiles[i];

                if (profile.x.length != profile.y.length) {
                    console.warn('Mismatched X and Y lengths in profile',profile);
                    continue;
                }

                this.ctx.strokeStyle = profile.color;

                if (profile.usePoints) {
                    for (let i = 1; i < profile.x.length; i++) {
                        this.ctx.strokeRect(
                            profile.x[i] * scale + offsetX - markerOffset,
                            - profile.y[i] * scale + offsetY - markerOffset,
                            this.markerWidth - 1,
                            this.markerWidth - 1);
                        this.ctx.lineTo(
                            profile.x[i] * scale + offsetX,
                            - profile.y[i] * scale + offsetY);
                    }
                }
                else {
                    this.ctx.beginPath();
                    this.ctx.moveTo(profile.x[0] * scale + offsetX, - profile.y[0] * scale + offsetY);

                    for (let i = 1; i < profile.x.length; i++) {
                        this.ctx.lineTo(profile.x[i] * scale + offsetX, - profile.y[i] * scale + offsetY);
                    }

                    this.ctx.closePath();
                    this.ctx.stroke();
                }
            }
        }
        else {
            console.warn(`Given profiles null`);
        }
    }
}