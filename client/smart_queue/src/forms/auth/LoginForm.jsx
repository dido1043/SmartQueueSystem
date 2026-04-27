import React from "react";
import { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
const apiUrl = import.meta.env.VITE_API_URL;

function LoginForm() {
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        email: "",
        password: ""
    });

    const [errors, setErrors] = useState({});

    const [handleError, setHandleError] = useState({
        status: '',
        message: ''
    });
    const [login, setLogin] = useState(false);

    const [handleSuccess, setHandleSuccess] = useState({
        status: 'Success',
        message: 'Successful login!'
    });

    const saveUserInfo = (authData) => {
        const userInfo = {
            accessToken: authData?.accessToken || '',
            expiration: authData?.expiration || '',
            refreshToken: authData?.refreshToken || '',
            userRole: authData?.userRole || '',
            userId: authData?.userId || ''
        };

        localStorage.setItem("userInfo", JSON.stringify(userInfo));
    };

    const persistGoogleAuthResponse = () => {
        const params = new URLSearchParams(window.location.search);

        const accessToken = params.get("accessToken");
        const expiration = params.get("expiration");
        const refreshToken = params.get("refreshToken");
        const userRole = params.get("userRole");
        const userId = params.get("UserId");

        if (!accessToken) return false;

        const responseJson = {
            accessToken,
            expiration,
            refreshToken,
            userRole,
            userId
        };

        saveUserInfo(responseJson);

        window.history.replaceState({}, document.title, window.location.pathname);
        return true;
    };

    useEffect(() => {
        if (persistGoogleAuthResponse()) {
            navigate('/');
        }
    }, [navigate]);

    const handleGoogleLogin = (e) => {
        e.preventDefault();
        if (persistGoogleAuthResponse()) {
            navigate('/');
            return;
        }

        const returnUrl = encodeURIComponent(`${window.location.origin}/login`);
        window.location.href = `${apiUrl}/api/Auth/google-login?returnUrl=${returnUrl}`;
    };
    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(`${apiUrl}/Api/auth/login`, formData);
            setHandleSuccess({
                status: 'Success',
                message: response.data.message || 'Successful login!'
            });
            setLogin(true);
            setTimeout(() => {
                navigate("/");
            }, 2000);

            saveUserInfo(response.data);
        } catch (error) {
            setHandleError({
                status: 'Error',
                message: error.response?.data?.message || 'Login failed. Please try again.'
            });
        }
    }

    const styles = {
        container: {
            maxWidth: "420px",
            margin: "40px auto",
            padding: "24px",
            border: "1px solid #e5e7eb",
            borderRadius: "12px",
            boxShadow: "0 8px 20px rgba(0,0,0,0.08)",
            background: "#fff"
        },
        title: {
            marginBottom: "16px",
            textAlign: "center"
        },
        fieldWrap: {
            marginBottom: "12px"
        },
        label: {
            display: "block",
            marginBottom: "6px",
            fontWeight: 500,
            color: "#111827"
        },
        input: {
            width: "100%",
            padding: "10px",
            border: "1px solid #d1d5db",
            borderRadius: "8px",
            fontSize: "14px"
        },
        button: {
            width: "100%",
            padding: "10px 14px",
            border: "none",
            borderRadius: "8px",
            background: "#2563eb",
            color: "#fff",
            cursor: "pointer",
            fontWeight: 600,
            marginTop: "4px"
        },
        errorText: {
            color: "#dc2626",
            fontSize: "12px",
            marginTop: "4px",
            display: "block"
        },
        errorBox: {
            marginBottom: "12px",
            padding: "10px",
            borderRadius: "8px",
            background: "#fee2e2",
            color: "#991b1b",
            fontSize: "14px"
        },
        successBox: {
            marginBottom: "12px",
            padding: "10px",
            borderRadius: "8px",
            background: "#dcfce7",
            color: "#166534",
            fontSize: "14px"
        }
    };

    return (
        <div style={styles.container}>
            <h2 style={styles.title}>Login</h2>

            {handleError?.status === 'Error' && (
                <div style={styles.errorBox}>{handleError.message}</div>
            )}

            {login && (
                <div style={styles.successBox}>{handleSuccess.message}</div>
            )}

            <form onSubmit={handleSubmit}>
                <div style={styles.fieldWrap}>
                    <label style={styles.label}>Email</label>
                    <input
                        type="email"
                        value={formData.email}
                        onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                        style={styles.input}
                    />
                    {errors.email && <span style={styles.errorText}>{errors.email}</span>}
                </div>

                <div style={styles.fieldWrap}>
                    <label style={styles.label}>Password</label>
                    <input
                        type="password"
                        value={formData.password}
                        onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                        style={styles.input}
                    />
                    {errors.password && <span style={styles.errorText}>{errors.password}</span>}
                </div>

                <button type="submit" style={styles.button}>Login</button>

                <div style={{ marginTop: "12px", textAlign: "center" }}>
                    <button
                        type="button"
                        onClick={handleGoogleLogin}
                        style={{
                            width: "100%",
                            padding: "10px 14px",
                            border: "1px solid #d1d5db",
                            borderRadius: "8px",
                            background: "#fff",
                            color: "#111827",
                            cursor: "pointer",
                            fontWeight: 600
                        }}
                    >
                        Login with Google
                    </button>
                </div>
            </form>
        </div>
    );
}

export default LoginForm;