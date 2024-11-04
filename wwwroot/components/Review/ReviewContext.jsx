import React, { createContext, useState, useEffect } from 'react';
import axios from 'axios';
import { toast, Slide } from 'react-toastify';
export const ReviewContext = createContext();
function getProductIdFromPath() {
    const pathParts = window.location.pathname.split('/');
    return pathParts[pathParts.length - 1];
}
const notify = (message) => {
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
export const ReviewProvider = ({ children }) => {

    const [currentUserId, setCurrentUserId] = useState('');
    const [productId] = useState(getProductIdFromPath() || 0);
    const [countReviews, setCountReviews] = useState(0);
    const [productReviews, setProductReviews] = useState([]);
    const [avgReviewRate, setAvgReviewRate] = useState(0);
    const [review, setReview] = useState([]);

    const fetchAllData = async (productId) => {

        fetchProductReviews(productId);
        fetchReviewsCount(productId);
        fetchAvgProductRate(productId);
        const userReview = await fetchCurrentReview(productId);
        setReview(userReview ? [userReview] : []);
    }

    const fecthCurrentUserId = async () => {

        const response = await axios.get(`/ReviewApi/GetCurrentUserId`);
        setCurrentUserId(response.data);

    };

    const fetchProductReviews = async (productId) => {


        const response = await axios.get(`/ReviewApi/GetProductReviews?productId=${productId}`);

        const newProductReviews = response.data.map(item => ({ ...item }));
        setProductReviews([...newProductReviews]);

    };

    const fetchReviewsCount = async (productId) => {


        const response = await axios.get(`/ReviewApi/GetCountProductReviews?productId=${productId}`);

        setCountReviews(response.data);

    };

    const fetchAvgProductRate = async (productId) => {


        const response = await axios.get(`/ReviewApi/GetAvgRateProduct?productId=${productId}`);

        setAvgReviewRate(response.data);

    };

    const fetchCurrentReview = async (productId) => {

        const response = await axios.get(`/ReviewApi/GetReview?productId=${productId}`);
        return response.data;

    };
    const AddReview = async (reviewDto) => {


        await axios.post('/ReviewApi/AddReview', reviewDto);
        notify('Отзыв оставлен!');
        fetchAllData(productId);

    };

    const UpdateReview = async (reviewDto) => {

        await axios.put('/ReviewApi/UpdateReview', reviewDto);
        notify('Отзыв успешно изменён');
        fetchAllData(productId);

    };

    const RemoveReview = async (productId) => {


        await axios.delete(`/ReviewApi/RemoveReview/${productId}`);
        notify('Отзыв успешно удален!');
        fetchAllData(productId);

    };
    useEffect(() => {
        fetchAllData(productId);
        fecthCurrentUserId();
    }, []);
    return (
        <ReviewContext.Provider
            value={{
                AddReview,
                productId,
                countReviews,
                productReviews,
                avgReviewRate,
                currentUserId,
                RemoveReview,
                UpdateReview,
                review
            }}
        >
            {children}
        </ReviewContext.Provider >
    );
};