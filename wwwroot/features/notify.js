import { toast, Slide } from 'react-toastify';
export const notifySuccess = (message) => {
    toast.success(message, {
        position: "bottom-right",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: false,
        draggable: true,
        theme: "colored",
        transition: Slide,
        className: 'my-custom-toast',
        bodyClassName: 'my-custom-body',
        progressClassName: 'my-custom-progress-bar'
    });
};
