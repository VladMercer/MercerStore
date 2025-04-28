import {useSelector} from 'react-redux';

export const useReviews = () => {


    const reviews = useSelector((state) => state.reviewPage.reviews);
    const pageNumber = useSelector((state) => state.reviewPage.pageNumber);
    const pageSize = useSelector((state) => state.reviewPage.pageSize);
    const totalPages = useSelector((state) => state.reviewPage.totalPages);
    const sortOrder = useSelector((state) => state.reviewPage.sortOrder);
    const totalReviews = useSelector((state) => state.reviewPage.totalReviews);
    const isLoaded = useSelector((state) => state.reviewPage.isLoaded);
    const isPageReset = useSelector((state) => state.reviewPage.isPageReset);
    const timePeriodFilter = useSelector((state) => state.reviewPage.timePeriodFilter);
    const filter = useSelector((state) => state.reviewPage.filter);
    const query = useSelector((state) => state.reviewPage.query);

    return {
        query,
        reviews,
        pageNumber,
        pageSize,
        totalPages,
        sortOrder,
        totalReviews,
        isLoaded,
        isPageReset,
        timePeriodFilter,
        filter,
    };
};