<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inbox.aspx.cs" Inherits="CIIRSNewVersion.Inbox" %>

<!DOCTYPE html>
<html lang="en">
<head>

  <!-- SITE TITTLE -->
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>Classimax</title>
  
  <!-- FAVICON -->
  <link href="img/favicon.png" rel="shortcut icon">
  <!-- PLUGINS CSS STYLE -->
  <!-- <link href="plugins/jquery-ui/jquery-ui.min.css" rel="stylesheet"> -->
  <!-- Bootstrap -->
  <link rel="stylesheet" href="plugins/bootstrap/css/bootstrap.min.css">
  <link rel="stylesheet" href="plugins/bootstrap/css/bootstrap-slider.css">
  <!-- Font Awesome -->
  <link href="plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet">
  <!-- Owl Carousel -->
  <link href="plugins/slick-carousel/slick/slick.css" rel="stylesheet">
  <link href="plugins/slick-carousel/slick/slick-theme.css" rel="stylesheet">
  <!-- Fancy Box -->
  <link href="plugins/fancybox/jquery.fancybox.pack.css" rel="stylesheet">
  <link href="plugins/jquery-nice-select/css/nice-select.css" rel="stylesheet">
  <!-- CUSTOM CSS -->
  <link href="css/style.css" rel="stylesheet">


  <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
  <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
  <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->

</head>

<body class="body-wrapper">

<!--===============================
=            Nav Bar              =
================================-->

<section>
	<div class="container">
		<div class="row">
			<div class="col-md-12">
				<nav class="navbar navbar-expand-lg navbar-light navigation">
					<a class="navbar-brand" href="index.html">
						<img src="images/logo.jfif" alt="">
					</a>
					<button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent"
					 aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
						<span class="navbar-toggler-icon"></span>
					</button>
					<div class="collapse navbar-collapse" id="navbarSupportedContent">
						<ul class="navbar-nav ml-auto main-nav ">
							<li class="nav-item active">
								<a class="nav-link" href="index.html">Home</a>
							</li>
							<li class="nav-item dropdown dropdown-slide">
								<a class="nav-link dropdown-toggle" data-toggle="dropdown" href="">Mailbox<span><i class="fa fa-angle-down"></i></span>
								</a>

								<!-- Dropdown list -->
								<div class="dropdown-menu">
									<a class="dropdown-item" href="dashboard.html">eCLP Inbox</a>
									<a class="dropdown-item" href="dashboard-my-ads.html">eIOM Inbox</a>
									<a class="dropdown-item" href="dashboard-favourite-ads.html">ESMS Inbox</a>
									<a class="dropdown-item" href="dashboard-archived-ads.html">Team Inbox</a>
									<%--<a class="dropdown-item" href="dashboard-pending-ads.html">Dashboard Pending Ads</a>--%>
								</div>
							</li>
							<li class="nav-item dropdown dropdown-slide">
								<a class="nav-link dropdown-toggle" href="#" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
									MISes <span><i class="fa fa-angle-down"></i></span>
								</a>
								<!-- Dropdown list -->
								<div class="dropdown-menu">
									<a class="dropdown-item" href="about-us.html">About Us</a>
									<a class="dropdown-item" href="contact-us.html">Contact Us</a>
									<a class="dropdown-item" href="user-profile.html">User Profile</a>
									<a class="dropdown-item" href="404.html">404 Page</a>
									<a class="dropdown-item" href="package.html">Package</a>
									<a class="dropdown-item" href="single.html">Single Page</a>
									<a class="dropdown-item" href="store.html">Store Single</a>
									<a class="dropdown-item" href="single-blog.html">Single Post</a>
									<a class="dropdown-item" href="blog.html">Blog</a>

								</div>
							</li>
							<li class="nav-item dropdown dropdown-slide">
								<a class="nav-link dropdown-toggle" href="" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
									Listing <span><i class="fa fa-angle-down"></i></span>
								</a>
								<!-- Dropdown list -->
								<div class="dropdown-menu">
									<a class="dropdown-item" href="category.html">Ad-Gird View</a>
									<a class="dropdown-item" href="ad-listing-list.html">Ad-List View</a>
								</div>
							</li>
						</ul>
						<ul class="navbar-nav ml-auto mt-10">
							<li class="nav-item">
								<a class="nav-link login-button" href="login.html">Login</a>
							</li>
							<li class="nav-item">
								<a class="nav-link text-white add-button" href="ad-listing.html"><i class="fa fa-plus-circle"></i> Add Listing</a>
							</li>
						</ul>
					</div>
				</nav>
			</div>
		</div>
	</div>
</section>

<!--===================================
=            Clients Section        =
====================================-->

<section class="client-slider-03">
	<!-- Container Start -->
	<div class="container">
		<div class="row">
			<!-- Client Slider -->
			<div class="col-md-12">
				<!-- Client Slider -->
<div class="category-slider">
    <!-- Client 01 -->
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-bed"></i>
            <h4>Bed</h4>
        </a>
    </div>
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-bed"></i>
            <h4>Hotels</h4>
        </a>
    </div>
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-car"></i>
            <h4>Cars</h4>
        </a>
    </div>
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-cutlery"></i>
            <h4>Restaurants</h4>
        </a>
    </div>
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-mobile"></i>
            <h4>Automobile</h4>
        </a>
    </div>
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-film"></i>
            <h4>Gym</h4>
        </a>
    </div>
    <!-- Client 01 -->
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-paragraph"></i>
            <h4>Park</h4>
        </a>
    </div>
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-play"></i>
            <h4>Play</h4>
        </a>
    </div>
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-building"></i>
            <h4>Real Estate</h4>
        </a>
    </div>
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-shopping-bag"></i>
            <h4>Shopping</h4>
        </a>
    </div>
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-bed"></i>
            <h4>Electronics</h4>
        </a>
    </div>
    <div class="item">
        <a href="#">
            <!-- Slider Image -->
            <i class="fa fa-bed"></i>
            <h4>Health</h4>
        </a>
    </div>
    
</div>
			</div>
		</div>
	</div>
	<!-- Container End -->
</section>

<section class="stores section">
	<div class="container">
		<div class="row">
            <div class="col-md-12 offset-md-1 col-lg-12 offset-lg-0">
				<!-- Recently Favorited -->
				<div class="widget dashboard-container my-adslist">
					<h3 class="widget-header">Inbox</h3>
					<table class="table table-responsive product-dashboard-table">
						<thead>
							<tr>
								<th>Sr.No</th>
								<th>Client No</th>
								<th class="text-center">eCLP No.</th>
								<th class="text-center">Client Name</th>
                                <th class="text-center">Branch</th>
                                <th class="text-center">eCLP Send By</th>
                                <th class="text-center">eCLP Recieving Date</th>
                                <th class="text-center">View</th>
                                <th class="text-center">Track</th>
                                <th class="text-center">Dispatch</th>
                                <th class="text-center">Send</th>
							</tr>
						</thead>
						<tbody>
							<tr>
								<td class="product-thumb">
                                    <span class="add-id"><strong>1.</strong></span>
									<%--<img width="80px" height="auto" src="images/products/products-1.jpg" alt="image description"></td>--%>
								<td class="product-details">
                                    <span class="categories">125789</span>
									<%--<h3 class="title">Macbook Pro 15inch</h3>
									<span class="add-id"><strong>Ad ID:</strong> ng3D5hAMHPajQrM</span>
									<span><strong>Posted on: </strong><time>Jun 27, 2017</time> </span>
									<span class="status active"><strong>Status</strong>Active</span>
									<span class="location"><strong>Location</strong>Dhaka,Bangladesh</span>--%>
								</td>
								<td class="product-category"><span class="categories">125789</span></td>
                                <td class="product-category"><span class="categories">ABC Company</span></td>
                                <td class="product-category"><span class="categories">BA Building</span></td>
                                <td class="product-category"><span class="categories">Rijaa</span></td>
                                <td class="product-category"><span class="categories">01-01-2020</span></td>
								<td class="action" data-title="Action">
									<div class="">
										<ul class="list-inline justify-content-center">
											<li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Edit" class="edit" href="">
													<i class="fa fa-clipboard"></i>
												</a>
											</li>
											<li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Delete" class="delete" href="">
													<i class="fa fa-trash"></i>
												</a>
											</li>
                                            <li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Edit" class="edit" href="">
													<i class="fa fa-clipboard"></i>
												</a>
											</li>
                                            <li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Edit" class="edit" href="">
													<i class="fa fa-clipboard"></i>
												</a>
											</li>
										</ul>
									</div>
								</td>
							</tr>
							<tr>

								<td class="product-thumb">
									<img width="80px" height="auto" src="images/products/products-2.jpg" alt="image description"></td>
								<td class="product-details">
									<h3 class="title">Study Table Combo</h3>
									<span class="add-id"><strong>Ad ID:</strong> ng3D5hAMHPajQrM</span>
									<span><strong>Posted on: </strong><time>Feb 12, 2017</time> </span>
									<span class="status active"><strong>Status</strong>Active</span>
									<span class="location"><strong>Location</strong>USA</span>
								</td>
								<td class="product-category"><span class="categories">Laptops</span></td>
								<td class="action" data-title="Action">
									<div class="">
										<ul class="list-inline justify-content-center">
											<li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Edit" class="edit" href="">
													<i class="fa fa-clipboard"></i>
												</a>
											</li>
											<li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Delete" class="delete" href="">
													<i class="fa fa-trash"></i>
												</a>
											</li>
										</ul>
									</div>
								</td>
							</tr>
							<tr>

								<td class="product-thumb">
									<img width="80px" height="auto" src="images/products/products-3.jpg" alt="image description"></td>
								<td class="product-details">
									<h3 class="title">Macbook Pro 15inch</h3>
									<span class="add-id"><strong>Ad ID:</strong> ng3D5hAMHPajQrM</span>
									<span><strong>Posted on: </strong><time>Jun 27, 2017</time> </span>
									<span class="status active"><strong>Status</strong>Active</span>
									<span class="location"><strong>Location</strong>Dhaka,Bangladesh</span>
								</td>
								<td class="product-category"><span class="categories">Laptops</span></td>
								<td class="action" data-title="Action">
									<div class="">
										<ul class="list-inline justify-content-center">
											<li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Edit" class="edit" href="">
													<i class="fa fa-clipboard"></i>
												</a>
											</li>
											<li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Delete" class="delete" href="">
													<i class="fa fa-trash"></i>
												</a>
											</li>
										</ul>
									</div>
								</td>
							</tr>
							<tr>

								<td class="product-thumb">
									<img width="80px" height="auto" src="images/products/products-4.jpg" alt="image description"></td>
								<td class="product-details">
									<h3 class="title">Macbook Pro 15inch</h3>
									<span class="add-id"><strong>Ad ID:</strong> ng3D5hAMHPajQrM</span>
									<span><strong>Posted on: </strong><time>Jun 27, 2017</time> </span>
									<span class="status active"><strong>Status</strong>Active</span>
									<span class="location"><strong>Location</strong>Dhaka,Bangladesh</span>
								</td>
								<td class="product-category"><span class="categories">Laptops</span></td>
								<td class="action" data-title="Action">
									<div class="">
										<ul class="list-inline justify-content-center">
											<li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Edit" class="edit" href="">
													<i class="fa fa-clipboard"></i>
												</a>
											</li>
											<li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Delete" class="delete" href="">
													<i class="fa fa-trash"></i>
												</a>
											</li>
										</ul>
									</div>
								</td>
							</tr>
							<tr>

								<td class="product-thumb">
									<img width="80px" height="auto" src="images/products/products-1.jpg" alt="image description"></td>
								<td class="product-details">
									<h3 class="title">Macbook Pro 15inch</h3>
									<span class="add-id"><strong>Ad ID:</strong> ng3D5hAMHPajQrM</span>
									<span><strong>Posted on: </strong><time>Jun 27, 2017</time> </span>
									<span class="status active"><strong>Status</strong>Active</span>
									<span class="location"><strong>Location</strong>Dhaka,Bangladesh</span>
								</td>
								<td class="product-category"><span class="categories">Laptops</span></td>
								<td class="action" data-title="Action">
									<div class="">
										<ul class="list-inline justify-content-center">
											<li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Edit" class="edit" href="">
													<i class="fa fa-clipboard"></i>
												</a>
											</li>
											<li class="list-inline-item">
												<a data-toggle="tooltip" data-placement="top" title="Delete" class="delete" href="">
													<i class="fa fa-trash"></i>
												</a>
											</li>
										</ul>
									</div>
								</td>
							</tr>
						</tbody>
					</table>

				</div>

				<!-- pagination -->
				<div class="pagination justify-content-center">
					<nav aria-label="Page navigation example">
						<ul class="pagination">
							<li class="page-item">
								<a class="page-link" href="#" aria-label="Previous">
									<span aria-hidden="true">&laquo;</span>
									<span class="sr-only">Previous</span>
								</a>
							</li>
							<li class="page-item"><a class="page-link" href="#">1</a></li>
							<li class="page-item active"><a class="page-link" href="#">2</a></li>
							<li class="page-item"><a class="page-link" href="#">3</a></li>
							<li class="page-item">
								<a class="page-link" href="#" aria-label="Next">
									<span aria-hidden="true">&raquo;</span>
									<span class="sr-only">Next</span>
								</a>
							</li>
						</ul>
					</nav>
				</div>
				<!-- pagination -->

			</div>
			<%--<div class="col-md-12">
				<div class="section-title">
					<h2>More Stores</h2>
				</div>
				<!-- First Letter -->
				<div class="block">
					<!-- Store First Letter -->
					<h5 class="store-letter">#</h5>
					<hr>
					<!-- Store Lists -->
					<div class="row">
						<!-- Store List 01 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 02 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 03 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 04 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
					</div>
				</div>
				<!-- Second Letter -->
				<div class="block">
					<!-- Store First Letter -->
					<h5 class="store-letter">A</h5>
					<hr>
					<!-- Store Lists -->
					<div class="row">
						<!-- Store List 01 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 02 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 03 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 04 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
					</div>
				</div>
				<!-- Third Letter -->
				<div class="block">
					<!-- Store First Letter -->
					<h5 class="store-letter">B</h5>
					<hr>
					<!-- Store Lists -->
					<div class="row">
						<!-- Store List 01 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 02 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 03 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 04 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
					</div>
				</div>
				<!-- Fourth Letter -->
				<div class="block">
					<!-- Store First Letter -->
					<h5 class="store-letter">C</h5>
					<hr>
					<!-- Store Lists -->
					<div class="row">
						<!-- Store List 01 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 02 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 03 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
						<!-- Store List 04 -->
						<div class="col-md-3 col-sm-6">
							<ul class="store-list">
								<li><a href="#">1 - 800 - Got - Junk?</a></li>
								<li><a href="#">1000 bulbs.com</a></li>
								<li><a href="#">180 packrat.com</a></li>
								<li><a href="#">3 day blinds</a></li>
							</ul>	
						</div>
					</div>
				</div>
			</div>--%>
		</div>
	</div>
</section>

<!--============================
=            Footer            =
=============================-->

<footer class="footer section section-sm">
  <!-- Container Start -->
  <div class="container">
    <div class="row">
      <div class="col-lg-3 col-md-7 offset-md-1 offset-lg-0">
        <!-- About -->
        <div class="block about">
          <!-- footer logo -->
          <img src="images/logo-footer.png" alt="">
          <!-- description -->
          <p class="alt-color">Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
            incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
            laboris nisi ut aliquip ex ea commodo consequat.</p>
        </div>
      </div>
      <!-- Link list -->
      <div class="col-lg-2 offset-lg-1 col-md-3">
        <div class="block">
          <h4>Site Pages</h4>
          <ul>
            <li><a href="#">Boston</a></li>
            <li><a href="#">How It works</a></li>
            <li><a href="#">Deals & Coupons</a></li>
            <li><a href="#">Articls & Tips</a></li>
            <li><a href="terms-condition.html">Terms & Conditions</a></li>
          </ul>
        </div>
      </div>
      <!-- Link list -->
      <div class="col-lg-2 col-md-3 offset-md-1 offset-lg-0">
        <div class="block">
          <h4>Admin Pages</h4>
          <ul>
            <li><a href="category.html">Category</a></li>
            <li><a href="single.html">Single Page</a></li>
            <li><a href="store.html">Store Single</a></li>
            <li><a href="single-blog.html">Single Post</a>
            </li>
            <li><a href="blog.html">Blog</a></li>



          </ul>
        </div>
      </div>
      <!-- Promotion -->
      <div class="col-lg-4 col-md-7">
        <!-- App promotion -->
        <div class="block-2 app-promotion">
          <div class="mobile d-flex">
            <a href="">
              <!-- Icon -->
              <img src="images/footer/phone-icon.png" alt="mobile-icon">
            </a>
            <p>Get the Dealsy Mobile App and Save more</p>
          </div>
          <div class="download-btn d-flex my-3">
            <a href="#"><img src="images/apps/google-play-store.png" class="img-fluid" alt=""></a>
            <a href="#" class=" ml-3"><img src="images/apps/apple-app-store.png" class="img-fluid" alt=""></a>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- Container End -->
</footer>
<!-- Footer Bottom -->
<footer class="footer-bottom">
  <!-- Container Start -->
  <div class="container">
    <div class="row">
      <div class="col-sm-6 col-12">
        <!-- Copyright -->
        <div class="copyright">
          <p>Copyright © <script>
              var CurrentYear = new Date().getFullYear()
              document.write(CurrentYear)
            </script>. All Rights Reserved, theme by <a class="text-primary" href="https://themefisher.com" target="_blank">themefisher.com</a></p>
        </div>
      </div>
      <div class="col-sm-6 col-12">
        <!-- Social Icons -->
        <ul class="social-media-icons text-right">
          <li><a class="fa fa-facebook" href="https://www.facebook.com/themefisher" target="_blank"></a></li>
          <li><a class="fa fa-twitter" href="https://www.twitter.com/themefisher" target="_blank"></a></li>
          <li><a class="fa fa-pinterest-p" href="https://www.pinterest.com/themefisher" target="_blank"></a></li>
          <li><a class="fa fa-vimeo" href=""></a></li>
        </ul>
      </div>
    </div>
  </div>
  <!-- Container End -->
  <!-- To Top -->
  <div class="top-to">
    <a id="top" class="" href="#"><i class="fa fa-angle-up"></i></a>
  </div>
</footer>

<!-- JAVASCRIPTS -->
<script src="plugins/jQuery/jquery.min.js"></script>
<script src="plugins/bootstrap/js/popper.min.js"></script>
<script src="plugins/bootstrap/js/bootstrap.min.js"></script>
<script src="plugins/bootstrap/js/bootstrap-slider.js"></script>
  <!-- tether js -->
<script src="plugins/tether/js/tether.min.js"></script>
<script src="plugins/raty/jquery.raty-fa.js"></script>
<script src="plugins/slick-carousel/slick/slick.min.js"></script>
<script src="plugins/jquery-nice-select/js/jquery.nice-select.min.js"></script>
<script src="plugins/fancybox/jquery.fancybox.pack.js"></script>
<script src="plugins/smoothscroll/SmoothScroll.min.js"></script>
<!-- google map -->
<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCcABaamniA6OL5YvYSpB3pFMNrXwXnLwU&libraries=places"></script>
<script src="plugins/google-map/gmap.js"></script>
<script src="js/script.js"></script>

</body>

</html>
