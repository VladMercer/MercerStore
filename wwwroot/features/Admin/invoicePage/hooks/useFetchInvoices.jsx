import { useDispatch } from 'react-redux';
import { useEffect, useRef } from 'react';
import { setPageNumber, setIsPageReset, fetchInvoices } from '../redux/invoicePageSlice';
import { useInvoices } from './useInvoices';


const useFetchInvoices = () => {
    const dispatch = useDispatch();
    const { pageNumber, pageSize, sortOrder, timePeriodFilter, filter, isLoaded, isPageReset, query } = useInvoices();

    const prevSortOrder = useRef(sortOrder);
    const prevQuery = useRef(query);

    const dispatchFetchInvoices = () => {
        dispatch(fetchInvoices({
            pageNumber, pageSize, sortOrder, timePeriodFilter, filter, query
        }));
    }

    useEffect(() => {
        

            if (!isLoaded) {
                dispatchFetchInvoices();
            }

            else if (pageNumber > 1 && (sortOrder !== prevSortOrder.current || query !== prevQuery.current) && !isPageReset) {
                dispatch(setPageNumber(1));
                dispatch(setIsPageReset(true));
            }

            else if (!isPageReset || (isPageReset && pageNumber === 1)) {

                dispatchFetchInvoices();

                if (isPageReset && pageNumber === 1) {
                    dispatch(setIsPageReset(false));
                }
            }

        prevSortOrder.current = sortOrder;
        prevQuery.current = query; 
    }, [pageNumber, pageSize, sortOrder, timePeriodFilter, filter, query]);
};

export default useFetchInvoices;