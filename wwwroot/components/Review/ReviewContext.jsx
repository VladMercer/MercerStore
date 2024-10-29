import React, { createContext, useState, useEffect } from 'react';
import axios from 'axios';
import { toast, Slide } from 'react-toastify';
export const ReviewContext = createContext();
function getProductIdFromPath() {
    const pathParts = window.location.pathname.split('/');
    return pathParts[pathParts.length - 1];
}

export const ReviewProvider = ({ children }) => {

    const [currentUserId, setCurrentUserId] = useState('');
    const [productId] = useState(getProductIdFromPath() || 0);
    const [countReviews, setCountReviews] = useState(0);
    const [productReviews, setProductReviews] = useState([]);
    const [avgReviewRate, setAvgReviewRate] = useState(0);
    const [review, setReview] = useState([]);

    const fetchAllData = async(productId) => {
        
        fetchProductReviews(productId);
        fetchReviewsCount(productId);
        fetchAvgProductRate(productId);
        const userReview = await fetchCurrentReview(productId);
        setReview(userReview ? [userReview] : []);
    }

    const fecthCurrentUserId = async () => {
        try {
            const response = await axios.get(`/ReviewApi/GetCurrentUserId`);
            console.log('Текущий пользователь ID:', response.data);
            setCurrentUserId(response.data);
        } catch (error) {
            console.error('Ошибка при получении ID пользователя:', error);
        }
    };

    const fetchProductReviews = async (productId) => {
        try {
            console.log(`Запрашиваем отзывы для продукта с ID: ${productId}`);
            const response = await axios.get(`/ReviewApi/GetProductReviews?productId=${productId}`);
            console.log('Отзывы продукта:', response.data);
            const newProductReviews = response.data.map(item => ({ ...item }));
            setProductReviews([...newProductReviews]);
        } catch (error) {
            console.error('Ошибка при получении отзывов:', error);
        }
    };

    const fetchReviewsCount = async (productId) => {
        try {
            console.log(`Запрашиваем количество отзывов для продукта с ID: ${productId}`);
            const response = await axios.get(`/ReviewApi/GetCountProductReviews?productId=${productId}`);
            console.log('Количество отзывов:', response.data);
            setCountReviews(response.data);
        } catch (error) {
            console.error('Ошибка при получении количества отзывов:', error);
        }
    };

    const fetchAvgProductRate = async (productId) => {
        try {
            console.log(`Запрашиваем среднюю оценку для продукта с ID: ${productId}`);
            const response = await axios.get(`/ReviewApi/GetAvgRateProduct?productId=${productId}`);
            console.log('Средняя оценка продукта:', response.data);
            setAvgReviewRate(response.data);
        } catch (error) {
            console.error('Ошибка при получении средней оценки товара:', error);
        }
    };

    const fetchCurrentReview = async (productId) => {
        try {
            const response = await axios.get(`/ReviewApi/GetReview?productId=${productId}`);
            return response.data;
        } catch (error) {
            console.error('Ошибка при получении отзыва пользователя', error);
        }
    };
    const AddReview = async (reviewDto) => {
        try {
            console.log("ПОЛУЧЕННАЯ ДТОШКА", reviewDto)
            await axios.post('/ReviewApi/AddReview', reviewDto);
            toast.success('Отзыв оставлен!', {
                position: "bottom-right",
                autoClose: 3000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: false,
                draggable: true,
                progress: undefined,
                theme: "colored",
                transition: Slide,
                className: 'my-custom-toast',
                bodyClassName: 'my-custom-body',
                progressClassName: 'my-custom-progress-bar'
            });
            fetchAllData(productId);
        }
        catch (error) {
            console.error('Ошибка при добавлении отзыва', error);

        }
    };

    const UpdateReview = async (reviewDto) => {
        try {
 
            await axios.put('/ReviewApi/UpdateReview', reviewDto);
            toast.success('Отзыв успешно отредактирован!', {
                position: "bottom-right",
                autoClose: 3000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: false,
                draggable: true,
                progress: undefined,
                theme: "colored",
                transition: Slide,
                className: 'my-custom-toast',
                bodyClassName: 'my-custom-body',
                progressClassName: 'my-custom-progress-bar'
            });
            fetchAllData(productId);
        } catch (error) {
            console.error('Ошибка при обновлении отзыва:', error);
        }
    };

    const RemoveReview = async (productId) => {
        try {

            await axios.delete(`/ReviewApi/RemoveReview/${productId}`);
            toast.success('Отзыв успешно удален!', {
                position: "bottom-right",
                autoClose: 3000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: false,
                draggable: true,
                progress: undefined,
                theme: "colored",
                transition: Slide,
                className: 'my-custom-toast',
                bodyClassName: 'my-custom-body',
                progressClassName: 'my-custom-progress-bar'
            });
            fetchAllData(productId);
        } catch (error) {
            console.error('Ошибка при удалении отзыва:', error);
        }
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