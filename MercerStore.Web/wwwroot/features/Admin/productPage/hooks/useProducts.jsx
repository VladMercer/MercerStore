import {useSelector} from 'react-redux';

export const useProducts = () => {

    const categoryId = useSelector((state) => state.adminProductPage.categoryId);
    const products = useSelector((state) => state.adminProductPage.products);
    const pageNumber = useSelector((state) => state.adminProductPage.pageNumber);
    const pageSize = useSelector((state) => state.adminProductPage.pageSize);
    const totalPages = useSelector((state) => state.adminProductPage.totalPages);
    const sortOrder = useSelector((state) => state.adminProductPage.sortOrder);
    const totalProducts = useSelector((state) => state.adminProductPage.totalProducts);
    const isLoaded = useSelector((state) => state.adminProductPage.isLoaded);
    const isPageReset = useSelector((state) => state.adminProductPage.isPageReset);
    const filter = useSelector((state) => state.adminProductPage.filter);
    const categories = useSelector((state) => state.adminProductPage.categories);

    return {
        products,
        pageNumber,
        pageSize,
        totalPages,
        sortOrder,
        categoryId,
        totalProducts,
        isLoaded,
        isPageReset,
        filter,
        categories
    };
};