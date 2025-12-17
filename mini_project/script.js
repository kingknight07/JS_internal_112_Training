// Get form element
const form = document.getElementById('registrationForm');

// Form submit event
form.addEventListener('submit', function (e) {
    // Prevent page reload
    e.preventDefault();

    // Clear previous errors
    clearErrors();

    // Get form values
    const name = document.getElementById('name').value.trim();
    const email = document.getElementById('email').value.trim();
    const gender = document.querySelector('input[name="gender"]:checked');
    const course = document.getElementById('course').value;
    const terms = document.getElementById('terms').checked;

    // Validate form
    let isValid = true;

    // Name validation
    if (name === '') {
        showError('nameError', '⚠ Identity required');
        isValid = false;
    }

    // Email validation
    if (!isValidEmail(email)) {
        showError('emailError', '⚠ Invalid comm frequency');
        isValid = false;
    }

    // Gender validation
    if (!gender) {
        showError('genderError', '⚠ Classification required');
        isValid = false;
    }

    // Course validation
    if (course === '') {
        showError('courseError', '⚠ Select training program');
        isValid = false;
    }

    // Terms validation
    if (!terms) {
        showError('termsError', '⚠ Agreement required');
        isValid = false;
    }

    // If all valid, start DOOM PROTOCOL
    if (isValid) {
        startDoomProtocol(name, email, gender.value, course);
    }
});

// Function to validate email
function isValidEmail(email) {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
}

// Function to show error
function showError(elementId, message) {
    const el = document.getElementById(elementId);
    el.textContent = message;
    anime({
        targets: el,
        translateX: [-5, 5, -5, 5, 0],
        duration: 400,
        easing: 'easeInOutQuad'
    });
}

// Function to clear all errors
function clearErrors() {
    document.getElementById('nameError').textContent = '';
    document.getElementById('emailError').textContent = '';
    document.getElementById('genderError').textContent = '';
    document.getElementById('courseError').textContent = '';
    document.getElementById('termsError').textContent = '';
}

// DR. DOOM PROTOCOL - Hacking Animation
function startDoomProtocol(name, email, gender, course) {
    const container = document.querySelector('.container');

    // Flash and glitch effect
    document.body.style.filter = 'hue-rotate(90deg) brightness(1.5)';
    setTimeout(() => {
        document.body.style.filter = 'none';
    }, 100);

    // Hide the form with glitch effect
    anime({
        targets: '#registrationForm',
        opacity: [1, 0],
        translateY: [0, -20],
        scale: [1, 0.95],
        duration: 400,
        easing: 'easeInQuad',
        complete: function () {
            // Replace form with DOOM hacking screen
            document.getElementById('registrationForm').style.display = 'none';

            // Change title to DOOM
            const title = document.querySelector('h1');
            title.innerHTML = '💀 DOOM PROTOCOL 💀';
            title.style.color = '#00ff00';
            title.style.textShadow = '0 0 20px #00ff00, 0 0 40px #00ff00';

            // Create hacking interface
            container.innerHTML += `
                <div id="hackingInterface">
                    <div class="doom-header">
                        <span class="doom-status">◈ SYSTEM BREACH INITIATED ◈</span>
                        <div class="doom-skull">💀</div>
                    </div>
                    
                    <div class="hack-terminal">
                        <div class="terminal-line" id="line1">> Accessing mainframe...</div>
                        <div class="terminal-line" id="line2"></div>
                        <div class="terminal-line" id="line3"></div>
                        <div class="terminal-line" id="line4"></div>
                    </div>
                    
                    <div class="data-extraction">
                        <div class="extract-title">◈ EXTRACTING DATA ◈</div>
                        <div class="data-row"><span class="data-label">TARGET:</span> <span class="data-value encrypt-value" data-final="${name}">██████████</span></div>
                        <div class="data-row"><span class="data-label">COMM:</span> <span class="data-value encrypt-value" data-final="${email}">██████████████</span></div>
                        <div class="data-row"><span class="data-label">CLASS:</span> <span class="data-value encrypt-value" data-final="${gender}">██████</span></div>
                        <div class="data-row"><span class="data-label">PROGRAM:</span> <span class="data-value encrypt-value" data-final="${course}">██████████</span></div>
                    </div>
                    
                    <div class="doom-progress">
                        <div class="progress-label">UPLOAD PROGRESS</div>
                        <div class="progress-bar"><div class="progress-fill" id="doomProgress"></div></div>
                        <div class="progress-percent">0%</div>
                    </div>
                    
                    <div class="doom-footer">
                        <span class="blink-text">◈ DOOM CONQUERS ALL ◈</span>
                    </div>
                </div>
            `;

            // Start the hacking animation sequence
            runHackingSequence(name, email, gender, course);
        }
    });
}

function runHackingSequence(name, email, gender, course) {
    // Animate hacking interface entrance
    anime({
        targets: '#hackingInterface',
        opacity: [0, 1],
        duration: 500,
        easing: 'easeOutQuad'
    });

    // Skull animation
    anime({
        targets: '.doom-skull',
        scale: [0, 1.5, 1],
        rotate: [0, 360],
        duration: 1000,
        easing: 'easeOutElastic(1, 0.5)'
    });

    // Terminal typing effect
    const terminalLines = [
        { id: 'line2', text: '> Bypassing security...', delay: 800 },
        { id: 'line3', text: '> Encrypting data stream...', delay: 1600 },
        { id: 'line4', text: '> DOOM PROTOCOL ACTIVE', delay: 2400 }
    ];

    terminalLines.forEach(line => {
        setTimeout(() => {
            typeWriter(line.id, line.text);
        }, line.delay);
    });

    // Data extraction animation
    setTimeout(() => {
        anime({
            targets: '.data-extraction',
            opacity: [0, 1],
            translateY: [20, 0],
            duration: 500,
            easing: 'easeOutQuad'
        });

        // Matrix encryption on data values
        const encryptValues = document.querySelectorAll('.encrypt-value');
        const cyberChars = '!@#$%^&*01アイウエオカキクケコ█▓▒░◈◆◇';

        encryptValues.forEach((el, index) => {
            const finalText = el.dataset.final;
            let iteration = 0;
            const maxIterations = 25;

            setTimeout(() => {
                const interval = setInterval(() => {
                    let displayText = '';
                    for (let i = 0; i < finalText.length; i++) {
                        if (iteration > maxIterations - 10 && i < (iteration - (maxIterations - 10))) {
                            displayText += finalText[i];
                        } else {
                            displayText += cyberChars[Math.floor(Math.random() * cyberChars.length)];
                        }
                    }
                    el.textContent = displayText;

                    iteration++;
                    if (iteration >= maxIterations) {
                        clearInterval(interval);
                        el.textContent = finalText;
                        el.style.color = '#00ff00';
                        el.style.textShadow = '0 0 10px #00ff00';
                    }
                }, 50);
            }, index * 500 + 500);
        });
    }, 2500);

    // Animate progress bar
    let progress = 0;
    const progressInterval = setInterval(() => {
        progress += 2;
        document.querySelector('.progress-percent').textContent = progress + '%';
        document.getElementById('doomProgress').style.width = progress + '%';

        if (progress >= 100) {
            clearInterval(progressInterval);
            // Transition to next page
            setTimeout(() => {
                transitionToSuccess(name, email, gender, course);
            }, 500);
        }
    }, 80);
}

function typeWriter(elementId, text) {
    const el = document.getElementById(elementId);
    let i = 0;
    el.style.opacity = '1';
    const interval = setInterval(() => {
        el.textContent += text[i];
        i++;
        if (i >= text.length) {
            clearInterval(interval);
        }
    }, 30);
}

function transitionToSuccess(name, email, gender, course) {
    // DOOM victory animation
    anime({
        targets: '.doom-skull',
        scale: [1, 2],
        opacity: [1, 0],
        duration: 500,
        easing: 'easeInQuad'
    });

    // Screen flash
    document.body.style.filter = 'brightness(3) hue-rotate(120deg)';

    setTimeout(() => {
        document.body.style.filter = 'brightness(1)';

        anime({
            targets: '.container',
            scale: [1, 0],
            rotate: [0, 180],
            opacity: [1, 0],
            duration: 800,
            easing: 'easeInBack',
            complete: function () {
                const params = new URLSearchParams();
                params.append('name', name);
                params.append('email', email);
                params.append('gender', gender);
                params.append('course', course);
                window.location.href = 'new.html?' + params.toString();
            }
        });
    }, 200);
}
