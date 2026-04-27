import React from "react";
import { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
const apiUrl = import.meta.env.VITE_API_URL;

function RegisterForm() {
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        name: "",
        email: "",
        password: "",
        role: ""
    });

    const [errors, setErrors] = useState({});

    const [handleError, setHandleError] = useState({
        status: '',
        message: ''
    });
    const [register, setRegister] = useState(false);

    const [handleSuccess, setHandleSuccess] = useState({
        status: 'Success',
        message: 'Successful register!'
    })
    const saveUserInfo = (authData) => {
        const userInfo = {
            accessToken: authData?.accessToken || '',
            expiration: authData?.expiration || '',
            refreshToken: authData?.refreshToken || '',
            userRole: authData?.userRole || ''
        };

        localStorage.setItem("userInfo", JSON.stringify(userInfo));
    };

    const persistGoogleAuthResponse = () => {
        const params = new URLSearchParams(window.location.search);

        const accessToken = params.get("accessToken");
        const expiration = params.get("expiration");
        const refreshToken = params.get("refreshToken");
        const userRole = params.get("userRole");

        if (!accessToken) return false;

        const responseJson = {
            accessToken,
            expiration,
            refreshToken,
            userRole
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

    const handleChange = (e) => {
        const name = e.target.name
        const value = e.target.value
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const validateForm = () => {
        const errors = {};
        if (!formData.email) errors.email = 'Email is required';
        if (!formData.password) errors.password = 'Password is required';
        if (!formData.name) errors.name = 'Name is required';
        if (!formData.role) errors.role = 'Role is required';
        return errors;
    };
    const handleSubmit = (e) => {
        setErrors({});
        e.preventDefault();
        const formErrors = validateForm();
        if (Object.keys(formErrors).length > 0) {
            setErrors(formErrors);
            return;
        }
        axios.post(`${apiUrl}/api/Auth/register`, formData, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': '*/*'
            }
        }).then((response) => {
            setRegister(true)
            setHandleSuccess({
                status: 'Success',
                message: 'Successful register!'
            })
            navigate('/login')
        }).catch((error) => {
            console.log(error.response.data.message);
            setHandleError({
                status: 'Error',
                message: error.response.data.message
            })
        });
    };

    const handleGoogleRegister = (e) => {
        e.preventDefault();
        if (persistGoogleAuthResponse()) {
            navigate('/');
            return;
        }

        const returnUrl = encodeURIComponent(`${window.location.origin}/register`);
        window.location.href = `${apiUrl}/api/Auth/google-login?returnUrl=${returnUrl}`;
    };
    return (
        <div
            style={{
                maxWidth: "420px",
                margin: "40px auto",
                padding: "24px",
                border: "1px solid #e5e7eb",
                borderRadius: "12px",
                boxShadow: "0 8px 20px rgba(0,0,0,0.08)",
                background: "#fff"
            }}
        >
            <h2 style={{ marginBottom: "16px", textAlign: "center" }}>Create Account</h2>

            {handleError?.status === "Error" && (
                <div
                    style={{
                        marginBottom: "12px",
                        padding: "10px",
                        borderRadius: "8px",
                        background: "#fee2e2",
                        color: "#991b1b",
                        fontSize: "14px"
                    }}
                >
                    {handleError.message}
                </div>
            )}

            {register && (
                <div
                    style={{
                        marginBottom: "12px",
                        padding: "10px",
                        borderRadius: "8px",
                        background: "#dcfce7",
                        color: "#166534",
                        fontSize: "14px"
                    }}
                >
                    {handleSuccess.message}
                </div>
            )}

            <form onSubmit={handleSubmit}>
                <div style={{ marginBottom: "12px" }}>
                    <label htmlFor="name" style={{ display: "block", marginBottom: "6px" }}>
                        Name
                    </label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        style={{
                            width: "100%",
                            padding: "10px",
                            border: "1px solid #d1d5db",
                            borderRadius: "8px"
                        }}
                    />
                    {errors.name && <small style={{ color: "#dc2626" }}>{errors.name}</small>}
                </div>

                <div style={{ marginBottom: "12px" }}>
                    <label htmlFor="email" style={{ display: "block", marginBottom: "6px" }}>
                        Email
                    </label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        style={{
                            width: "100%",
                            padding: "10px",
                            border: "1px solid #d1d5db",
                            borderRadius: "8px"
                        }}
                    />
                    {errors.email && <small style={{ color: "#dc2626" }}>{errors.email}</small>}
                </div>

                <div style={{ marginBottom: "12px" }}>
                    <label htmlFor="password" style={{ display: "block", marginBottom: "6px" }}>
                        Password
                    </label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        style={{
                            width: "100%",
                            padding: "10px",
                            border: "1px solid #d1d5db",
                            borderRadius: "8px"
                        }}
                    />
                    {errors.password && <small style={{ color: "#dc2626" }}>{errors.password}</small>}
                </div>

                <div style={{ marginBottom: "16px" }}>
                    <label htmlFor="role" style={{ display: "block", marginBottom: "6px" }}>
                        Role
                    </label>
                    <select
                        id="role"
                        name="role"
                        value={formData.role}
                        onChange={handleChange}
                        style={{
                            width: "100%",
                            padding: "10px",
                            border: "1px solid #d1d5db",
                            borderRadius: "8px"
                        }}
                    >
                        <option value="">Select role</option>
                        <option value="Client">Client</option>
                        <option value="Owner">Owner</option>
                    </select>
                    {errors.role && <small style={{ color: "#dc2626" }}>{errors.role}</small>}
                </div>

                <button
                    type="submit"
                    style={{
                        width: "100%",
                        padding: "10px 14px",
                        border: "none",
                        borderRadius: "8px",
                        background: "#2563eb",
                        marginBottom: "1rem",
                        color: "#fff",
                        cursor: "pointer",
                        fontWeight: 600
                    }}
                >
                    Register
                </button>

                <button
                    type="button"
                    style={{
                        width: "100%",
                        padding: "10px 14px",
                        border: "none",
                        borderRadius: "8px",
                        background: "#5c8ef9ff",
                        color: "#fff",
                        cursor: "pointer",
                        fontWeight: 600
                    }}
                    onClick={handleGoogleRegister}
                >
                    Register with Google
                </button>
            </form>
        </div>
    );
}

export default RegisterForm;
