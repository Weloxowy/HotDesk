import axiosInstance from "./AxiosInstance.tsx";


async function LogoutUser() {
    try {
        const response = await axiosInstance.post('auth/logout', {}, {});

        return response.data;
    } finally {
        window.location.href = "/";
    }
}

export default LogoutUser;
