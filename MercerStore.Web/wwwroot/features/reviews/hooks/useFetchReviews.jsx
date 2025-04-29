import {useDispatch} from 'react-redux';
import {useEffect} from 'react';
import {useReviews} from './useReviews';

import {
    fetchAvgProductRate,
    fetchCurrentReview,
    fetchCurrentUserId,
    fetchCurrentUserRoles,
    fetchProductReviews,
    fetchReviewsCount,
    setProductId
} from '../redux/reviewSlice';

const useFetchReviews = () => {
    const {isLoaded, productId} = useReviews();
    const dispatch = useDispatch();

    const getProductIdFromPath = () => {

        const pathParts = window.location.pathname.split('/');
        dispatch(setProductId(pathParts[pathParts.length - 1]));

    }

    useEffect(() => {
        if (!isLoaded) {
            getProductIdFromPath();
        }
        if (isLoaded) {
            dispatch(fetchProductReviews(productId));
            dispatch(fetchCurrentReview(productId));
            dispatch(fetchReviewsCount(productId));
            dispatch(fetchAvgProductRate(productId));
            dispatch(fetchCurrentUserId());
            dispatch(fetchCurrentUserRoles());
        }
    }, [isLoaded]);
};

export default useFetchReviews;