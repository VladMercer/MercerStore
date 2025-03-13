import { useDispatch } from 'react-redux';
import { useEffect, useRef } from 'react';
import { setPageNumber, setIsPageReset, fetchOrders } from '../redux/orderPageSlice';
import { useOrders } from './useOrders';


const useFetchOrders = () => {
    const dispatch = useDispatch();
    const { pageNumber, pageSize, sortOrder, timePeriodFilter, statusFilter, isLoaded, isPageReset, query } = useOrders();

    const prevSortOrder = useRef(sortOrder);
    const prevQuery = useRef(prevQuery);

    const dispatchFetchOrders = () => {
        dispatch(fetchOrders({
            pageNumber, pageSize, sortOrder, timePeriodFilter, statusFilter, query
        }));
    }

    useEffect(() => {
        

            if (!isLoaded) {
                dispatchFetchOrders();
            }

            else if (pageNumber > 1 && (sortOrder !== prevSortOrder.current || query !== prevQuery.current) && !isPageReset) {
                dispatch(setPageNumber(1));
                dispatch(setIsPageReset(true));
            }

            else if (!isPageReset || (isPageReset && pageNumber === 1)) {

                dispatchFetchOrders();

                if (isPageReset && pageNumber === 1) {
                    dispatch(setIsPageReset(false));
                }
            }
            prevQuery.current = query; 
            prevSortOrder.current = sortOrder;
    }, [pageNumber, pageSize, sortOrder, timePeriodFilter, statusFilter, query]);
};

export default useFetchOrders;