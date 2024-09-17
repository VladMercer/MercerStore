import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom';

const SearchComponent = () => {
    const [query, setQuery] = useState('');
    const [results, setResults] = useState([]);
    const [isDropdownVisible, setDropdownVisible] = useState(false);

    useEffect(() => {
        if (query.length < 2) {
            setResults([]);
            setDropdownVisible(false); 
            return;
        }

        const fetchData = async () => {
            try {
                const response = await fetch(`/Search/Search?query=${encodeURIComponent(query)}`);
                const data = await response.json();
                setResults(data);
                setDropdownVisible(true); 
            } catch (error) {
                console.error('Ошибка при получении данных:', error);
            }
        };

        fetchData();
    }, [query]);

    const handleSubmit = (e) => {
        e.preventDefault();
        window.location.href = `/Search/Search?query=${encodeURIComponent(query)}`; 
    };


    return (
        <div className="position-relative">
            <form onSubmit={handleSubmit} className="d-flex input-group">
                <input
                    type="text"
                    className="form-control"
                    value={query}
                    onChange={(e) => setQuery(e.target.value)}
                    placeholder="Поиск"
                    aria-label="Search"
                    aria-describedby="button-search"
                    onFocus={() => setDropdownVisible(true)} 
                   
                />
                <button className="btn btn-outline-warning" type="submit" id="button-search">
                    <i className="fa-solid fa-magnifying-glass"></i>
                </button>
            </form>
            {isDropdownVisible && results.length > 0 && (
                <div className="search-dropdown position-absolute bg-white border rounded shadow-sm w-100">
                    {results.map(product => (
                        <div
                            key={product.id}
                            className="search-result-item d-flex align-items-center p-2"
                            onClick={() => window.location.href = `/product/details/${product.id}`}
                            style={{ cursor: 'pointer' }}
                        >
                            <img src={product.imageUrl} alt={product.name} />
                            <div>
                                
                                <h5 dangerouslySetInnerHTML={{ __html: product.name }}></h5>
                                <p>{product.description}</p>
                               
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

ReactDOM.render(
    <SearchComponent />,
    document.getElementById('search-root')
);
//window.addEventListener('scroll', function () {
//    document.getElementById('header-nav').classList.toggle('headernav-scroll', window.scrollY > 133);
//});

const offcanvasCartEl = document.getElementById('offcanvasCart');
const offcanvasCart = new bootstrap.Offcanvas(offcanvasCartEl);

document.getElementById('cart-open').addEventListener('click', (e) => {
    e.preventDefault();
    offcanvasCart.toggle();
});

document.querySelectorAll('.closecart').forEach(item => {
    item.addEventListener('click', (e) => {
        e.preventDefault();
        offcanvasCart.hide();
        let href = item.dataset.href;
        document.getElementById(href).scrollIntoView();
    });
});


$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('#top').fadeIn();
        } else {
            $('#top').fadeOut();
        }
    });

    $('#top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 500);
        return false;
    });

    $(".owl-carousel-full").owlCarousel({
        margin: 20,
        responsive: {
            0: {
                items: 1
            },
            500: {
                items: 2
            },
            700: {
                items: 3
            },
            1000: {
                items: 4
            }
        }
    });
});