const { useState } = React;

function Alert(props) {
    if (!props.message) return null;
    const className = props.type === 'success' ? 'alert alert-success' : 'alert alert-danger';
    return React.createElement('div', {
        className: className,
        role: 'alert',
        style: { marginTop: '1rem' }
    }, props.message);
}

function SubscribeForm() {
    const [email, setEmail] = useState('');
    const [loading, setLoading] = useState(false);
    const [alert, setAlert] = useState({ message: '', type: '' });

    async function handleSubmit(e) {
        e.preventDefault();
        setAlert({ message: '', type: '' });

        if (!email || !email.includes('@')) {
            setAlert({ message: 'Please enter a valid email address.', type: 'danger' });
            return;
        }

        setLoading(true);
        try {
            const response = await fetch('/Home/Subscribe', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email })
            });

            const result = await response.json();

            if (result && result.success) {
                setAlert({
                    message: result.message || 'Thank you for subscribing! You will be notified about upcoming events.',
                    type: 'success'
                });
                setEmail('');
            } else {
                setAlert({
                    message: result.message || 'Subscription failed. Please try again.',
                    type: 'danger'
                });
            }
        } catch (err) {
            setAlert({
                message: 'An error occurred. Please try again later.',
                type: 'danger'
            });
            console.error(err);
        } finally {
            setLoading(false);
        }
    }

    return React.createElement('div', null,
        React.createElement('form', {
            id: 'subscriptionForm',
            className: 'subscribe-form',
            onSubmit: handleSubmit
        },
            React.createElement('div', { className: 'form-group' },
                React.createElement('label', {
                    className: 'form-label',
                    htmlFor: 'email'
                }, 'Email Address'),
                React.createElement('input', {
                    type: 'email',
                    id: 'email',
                    name: 'email',
                    className: 'form-input',
                    placeholder: 'Enter your email',
                    required: true,
                    value: email,
                    onChange: (e) => setEmail(e.target.value)
                })
            ),
            React.createElement('button', {
                type: 'submit',
                className: 'submit-btn',
                id: 'submitBtn',
                disabled: loading
            }, loading ? 'Subscribing...' : 'Subscribe Now')
        ),
        React.createElement(Alert, {
            message: alert.message,
            type: alert.type
        })
    );
}

// Mount компоненту
document.addEventListener('DOMContentLoaded', function () {
    const root = document.getElementById('subscribe-root');
    if (root) {
        ReactDOM.render(React.createElement(SubscribeForm), root);
    }
});