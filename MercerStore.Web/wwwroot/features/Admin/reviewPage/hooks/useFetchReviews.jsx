import {useDispatch} from 'react-redux';
import {useEffect, useRef} from 'react';
import {fetchReviews, setIsPageReset, setPageNumber} from '../redux/reviewPageSlice';
import {useReviews} from './useReviews';


const useFetchReviews = () => {
    const dispatch = useDispatch();
    const {pageNumber, pageSize, sortOrder, timePeriodFilter, filter, isLoaded, isPageReset, query} = useReviews();

    const prevSortOrder = useRef(sortOrder);
    const prevQuery = useRef(query);

    const dispatchFetchReviews = () => {
        dispatch(fetchReviews({
            pageNumber, pageSize, sortOrder, timePeriodFilter, filter, query
        }));
    }

    useEffect(() => {


        if (!isLoaded) {
            dispatchFetchReviews();
        } else if (pageNumber > 1 && (sortOrder !== prevSortOrder.current || query !== prevQuery.current) && !isPageReset) {
            dispatch(setPageNumber(1));
            dispatch(setIsPageReset(true));
        } else if (!isPageReset || (isPageReset && pageNumber === 1)) {

            dispatchFetchReviews();

            if (isPageReset && pageNumber === 1) {
                dispatch(setIsPageReset(false));
            }
        }

        prevSortOrder.current = sortOrder;
        prevQuery.current = query;
    }, [pageNumber, pageSize, sortOrder, timePeriodFilter, filter, query]);
};

export default useFetchReviews;