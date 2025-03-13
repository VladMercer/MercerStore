import { useSelector } from 'react-redux';

export const useReviews = () => {

    const productId = useSelector((state) => state.reviews.productId);
    const countReviews = useSelector((state) => state.reviews.countReviews);
    const productReviews = useSelector((state) => state.reviews.productReviews);
    const avgReviewRate = useSelector((state) => state.reviews.avgReviewRate);
    const currentUserId = useSelector((state) => state.reviews.currentUserId);
    const userRoles = useSelector((state) => state.reviews.userRoles);
    const review = useSelector((state) => state.reviews.review);
    const isLoaded = useSelector((state) => state.reviews.isLoaded);
 
    return {
        productId,
        countReviews,
        productReviews,
        avgReviewRate,
        currentUserId,
        userRoles,
        isLoaded,
        review,
    };
};



